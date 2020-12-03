Shader "Custom/CylinderShader"
{
    Properties
    {
       [NoScaleOffset] _Texture ("Texture", 2D) = "black" {}
	   _Contrast ("Contrast", Range(0,1)) = 0
	   _Intensity ("Intensity", Range(0,1)) = 0
	}
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off
        ZWrite Off
		Fog { Mode Off}
		Blend One One
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma  fragment frag

			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			uniform sampler2D _Texture;
			uniform float _Contrast;
			uniform float _Intensity;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = o.uv;
				return o;
			};
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 color = tex2D(_Texture,i.uv).rgb;
				return fixed4(lerp(color,color*color,_Contrast) *_Intensity,1);
			}
			ENDCG			
		}
    }
}
