Shader "Custom/newEasePointer" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Factor("show speed", Range(0,10)) = 10
	}

	SubShader{
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
		}
		LOD 200
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
#include "UnityCG.cginc"
#include "UnityUI.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
				float4 color: COLOR;
			};
			struct v2f
			{
				float4 vertex :	SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Factor;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
			{
				fixed4 color;
				float threshold = _Factor / 10.0;
				if (i.texcoord.x <= threshold)
				{
					color = tex2D(_MainTex, i.texcoord);
					color.a *= 0.8;
				}
				else
					color = (1.0, 1.0, 1.0, 0.0);
				return color;
			}
				ENDCG
		}

	}
}
