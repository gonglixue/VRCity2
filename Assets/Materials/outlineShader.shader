Shader "Custom/outlineShader" {
	Properties{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Outline("Outline",Range(0,1)) = 0.4
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _Outline;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal; INTERNAL_DATA
		};

		
		

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			half edge = saturate(dot(IN.worldNormal, normalize(IN.viewDir)));
			edge = edge < _Outline ? edge / 4 : 1;

			o.Albedo = c.rgb * edge;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
