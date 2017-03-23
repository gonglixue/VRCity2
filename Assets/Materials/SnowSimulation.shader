// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SnowSimulation" {
	Properties {
		_MainTex("Base Texture", 2D) = "white" {}
		_Bump("Bump", 2D) = "bump" {}  // 法线贴图
		_Snow("Snow Level", Range(0,1)) = 0
		_SnowColor("Snow Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_SnowDirection("Snow Direction", Vector) = (0,1,0)
		_SnowDepth("Snow Depth", Range(0, 0.3)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CustomDiffuse vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Bump;
		float _Snow;
		float4 _SnowColor;
		float4 _SnowDirection;
		float _SnowDepth;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Bump;
			float3 worldNormal; INTERNAL_DATA
		};

		void vert(inout appdata_full v)
		{
			// snowDirection 转世界坐标
			float4 sn = mul(transpose(unity_ObjectToWorld), _SnowDirection);
			// ? v.normal是世界坐标系法向量吗？
			if (dot(v.normal, sn.xyz) >= lerp(1, -1, (_Snow * 2) / 3))
			{
				v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow;
			}
		}


		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));

			if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) > lerp(1, -1, _Snow))
			{
				o.Albedo = _SnowColor.rgb;
			}
			else {
				o.Albedo = c.rgb;
			}

			o.Alpha = c.a;
		}

		inline float4 LightingCustomDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = max(0, dot(s.Normal, lightDir));
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2);
			col.a = s.Alpha;
			return col;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
