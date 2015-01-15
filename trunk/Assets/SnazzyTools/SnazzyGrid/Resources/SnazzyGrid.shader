
Shader "SnazzyTools/SnazzyGrid" {
    Properties {
        _GridColor ("GridColor", Color) = (0.5,0.5,0.5,1)
        _GridSteps ("GridSteps", Float ) = 1
        _Scale ("Scale", Float ) = 1
        _SnapAreaSize ("SnapAreaSize", Float ) = 5
        _SnapTransparency ("SnapTransparency", Float ) = 1
        _SnapOffset ("SnapOffset", Float ) = 0
        _StreakColor ("StreakColor", Color) = (0.7794118,0.7794118,0.7794118,1)
        _StreakScale ("StreakScale", Float ) = 50
        _SnazzyGrid1 ("SnazzyGrid1", 2D) = "white" {}
        _SnazzyGrid2 ("SnazzyGrid2", 2D) = "white" {}
        _GridOffset ("GridOffset", Float ) = 0.5
        _Fresnel ("Fresnel", Float ) = 10
        _VertexPush ("VertexPush", Float ) = 0.001
        _X ("X", Float ) = 0
        _Y ("Y", Float ) = 0
        _Z ("Z", Float ) = 0
        _ObjectScale ("ObjectScale", Float ) = 50
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
		Blend SrcAlpha OneMinusSrcAlpha 
        ZWrite Off
            
        Pass {
//          
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            // #pragma exclude_renderers xbox360 ps3 flash d3d11_9x
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Scale;
            uniform float _Fresnel;
            uniform float _SnapOffset;
            uniform float _VertexPush;
            uniform float _SnapTransparency;
            uniform sampler2D _SnazzyGrid2; uniform float4 _SnazzyGrid2_ST;
            uniform float _GridSteps;
            uniform sampler2D _SnazzyGrid1; uniform float4 _SnazzyGrid1_ST;
            uniform float _SnapAreaSize;
            uniform float _GridOffset;
            uniform float4 _GridColor;
            uniform float _StreakScale;
            uniform float4 _StreakColor;
            uniform float _X;
            uniform float _Y;
            uniform float _Z;
            uniform float _ObjectScale;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0; 
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                v.vertex.xyz += (_VertexPush*v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float3 normalDirection =  i.normalDir;

                float3 node_386 = i.normalDir;
                float3 node_384 = abs(node_386);
                float4 node_371 = i.posWorld;
                float2 node_392 = ((float2(node_371.g,node_371.b)/_Scale)+_SnapOffset);
                float4 node_372 = i.posWorld;
                float2 node_396 = ((float2(node_372.r,node_372.b)/_Scale)+_SnapOffset);
                float4 node_373 = i.posWorld;
                float2 node_400 = ((float2(node_373.r,node_373.g)/_Scale)+_SnapOffset);
                float node_1707 = 0.5;
                float2 node_1703 = ((i.uv0.rg*_SnapAreaSize)+(-1*((_SnapAreaSize*node_1707)-node_1707)));
                float node_64 = ((_SnapTransparency*(node_384.r*tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_392, _SnazzyGrid2)).b + node_384.g*tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_396, _SnazzyGrid2)).b + node_384.b*tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_400, _SnazzyGrid2)).b))*tex2D(_SnazzyGrid1,TRANSFORM_TEX(node_1703, _SnazzyGrid1)).b);
                float node_757 = 0.5;
                float2 node_638 = ((i.uv0.rg*_StreakScale)+(-1*((_StreakScale*node_757)-node_757)));
                float4 node_597 = tex2D(_SnazzyGrid1,TRANSFORM_TEX(node_638, _SnazzyGrid1));
                float node_1045 = 0.5;
                float2 node_1039 = (((i.uv0.rg*_ObjectScale)/_Scale)+(-1*(((_ObjectScale/_Scale)*node_1045)-node_1045)));
                float node_1256 = (_StreakScale*(0.2/_Scale));
                float node_1212 = 0.5;
                float2 node_1208 = ((i.uv0.rg*node_1256)+(-1*((node_1256*node_1212)-node_1212)));
                float node_1220 = (tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_1039, _SnazzyGrid2)).g*tex2D(_SnazzyGrid1,TRANSFORM_TEX(node_1208, _SnazzyGrid1)).g);
                float node_1221 = ((_StreakColor.a*node_597.g)+node_1220);
                float4 node_918 = _Time + _TimeEditor;
                float node_1317 = abs((node_1221*sin(node_918.g)));
                float3 emissive = lerp(lerp(_GridColor.rgb,abs(node_386),node_64),(_StreakColor.rgb*(node_597.g+node_1220)),node_1317);
                float3 finalColor = emissive;
                float2 node_1735 = i.uv0;
                float3 node_511 = abs(i.normalDir);
                float4 node_484 = i.posWorld;
                float2 node_496 = ((float2(node_484.g,node_484.b)/_GridSteps)+_GridOffset);
                float4 node_486 = i.posWorld;
                float2 node_500 = ((float2(node_486.r,node_486.b)/_GridSteps)+_GridOffset);
                float4 node_488 = i.posWorld;
                float2 node_504 = ((float2(node_488.r,node_488.g)/_GridSteps)+_GridOffset);

				return fixed4(finalColor,((tex2D(_SnazzyGrid1,TRANSFORM_TEX(node_1735.rg, _SnazzyGrid1)).b*lerp(lerp(((node_511.r*tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_496, _SnazzyGrid2)).r + node_511.g*tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_500, _SnazzyGrid2)).r + node_511.b*tex2D(_SnazzyGrid2,TRANSFORM_TEX(node_504, _SnazzyGrid2)).r)*_GridColor.a),node_64,node_64),node_1221,node_1317))*clamp((pow(dot(-i.normalDir,float3(float2(_X,_Y),_Z)),_Fresnel)*1.1+-0.1),0,1)));              
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
