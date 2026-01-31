Shader "Custom/SimpleOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,0.5,0,1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.005
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        
        Pass
        {
            Name "OUTLINE"
            Cull Front 
            ZWrite Off
            ZTest LEqual
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);

                o.pos.xy += offset * o.pos.z * _OutlineWidth;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}