Shader "Anwilc/CircleFade"
{
    Properties
    {
        _Color ("Tint", Color) = (1,1,1,1)
        _CircleSize ("Circle Size", Float) = 0.5
        _DownScaling ("Downscaling", int) = 1
        _FocusOffset("Focus offset", Vector) = (0, 0, 0, 0) 
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float4 focusWorld : TEXCOORD0;
                float2 uv : TEXCOORD2;
            };

            fixed4 _Color;
            float _CircleSize;
            float _DownScaling;
            float4 _FocusOffset;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.uv = v.texcoord;

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                if(_CircleSize < 0.01)
                    return _Color;

                float aspect = _ScreenParams.y / _ScreenParams.x;
				float2 offsetUV = IN.uv * _ScreenParams.xy - _FocusOffset;

				float u = (floor(offsetUV.x / _DownScaling)*_DownScaling) / _ScreenParams.x - 0.5;
				float v = (floor(offsetUV.y / _DownScaling)*_DownScaling) / _ScreenParams.y - 0.5;
                fixed4 color = fixed4(_Color.xyz, step(_CircleSize * _CircleSize, u*u / aspect + v*v));

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
