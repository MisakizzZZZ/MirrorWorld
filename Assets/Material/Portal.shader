Shader "Custom/Portal"
{
    Properties
    {
        _InactiveColour("Inactive Colour", Color) = (1, 1, 1, 1)
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Cull Front  //关闭背面剔除、这样在Portal内部屏幕中也能看到对面

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            sampler2D _MainTex;  //摄像机的画面将被渲染至MainTex
            int displayMask;
            float tempColor = (1, 1, 1, 1); //最深层迭代、 传送门设置为黑色

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);  
                o.screenPos = ComputeScreenPos(o.vertex);  //获取齐次坐标下的屏幕坐标值
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;  //计算屏幕视口UV坐标
                uv.x = 1.0-uv.x;
                return displayMask*tex2D(_MainTex, uv)+(1- displayMask)* tempColor;
            }
            ENDCG
        }
    }
        Fallback "Standard" // for shadows
}