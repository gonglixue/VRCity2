Shader "Custom/EasePointer"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Factor("show speed", Range(0,10)) = 10

	}

	SubShader
	{
		Tags{ "Queue"="Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _Factor;
		

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		half4 c;
		//float threshold = _Time.y * _Factor / 10.0;
		float threshold = _Factor / 10.0;
		if (IN.uv_MainTex.x <= threshold)
			c = tex2D(_MainTex, IN.uv_MainTex);
		else
			c = (1.0, 1.0, 1.0, 0.0);

		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}

	ENDCG
	}

		FallBack "Diffuse"
}
