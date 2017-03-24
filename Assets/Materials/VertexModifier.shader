Shader "Custom/VertexModifier" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Bump("Bump", 2D) = "bump" {}
		_Amount("Height Adjustment", Float) = 13025.33
		_HeightMap("Height Map",2D) = "black" {}
		_RelativeScale("Relative Scale",Float) = 0
		_SnowDirection("Snow Direction",Vector) = (0,1,0)
		_Snow("Snow Level", Range(0,1)) = 0.2
		_SnowColor("Snow Color",Color) = (1.0, 1.0, 1.0, 1.0)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma surface surf Lambert vertex:vert
		struct Input {
			float2 uv_MainTex;
			float2 uv_Bump;
			float3 worldNormal; INTERNAL_DATA
		};

	// Access the shaderlab properties
	float _Amount;
	sampler2D _MainTex;
	sampler2D _Bump;

	sampler2D _HeightMap;
	float _RelativeScale;

	float4 _SnowDirection;
	float _Snow;
	float4 _SnowColor;

	// Vertex modifier function
	void vert(inout appdata_full v) {
		// Do whatever you want with the "vertex" property of v here

		v.vertex.y += _Amount;
		float4 color = tex2Dlod(_HeightMap, v.texcoord);
		v.vertex.y += _RelativeScale*(-10000 + ((color.x * 255 * 256 * 256 + color.y * 255 * 256 + color.z * 255) * 0.1));
	}

	// Surface shader function
	void surf(Input IN, inout SurfaceOutput o) {

		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
		if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) > lerp(1, -1, _Snow))
		{
			o.Albedo = _SnowColor.rgb;
		}
		else
		{
			o.Albedo = c.rgb;
		}

		//o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
	}
	ENDCG
	}
}