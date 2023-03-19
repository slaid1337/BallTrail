Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _rModifire ("R", float) = 0.
        _gModifire ("G", float) = 0.
        _bModifire ("B", float) = 0.

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
        

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _rModifire;
            float _gModifire;
            float _bModifire;

        float colormap_red(float x) {
            if (x < 0.0) {
                return 54.0 / 255.0;
            } else if (x < 20049.0 / 82979.0) {
                return (829.79 * x + 54.51) / 255.0;
            } else {
                return 1.0;
            }
        }

        float colormap_green(float x) {
            if (x < 20049.0 / 82979.0) {
                return 0.0;
            } else if (x < 327013.0 / 810990.0) {
                return (8546482679670.0 / 10875673217.0 * x - 2064961390770.0 / 10875673217.0) / 255.0;
            } else if (x <= 1.0) {
                return (103806720.0 / 483977.0 * x + 19607415.0 / 483977.0) / 255.0;
            } else {
                return 1.0;
            }
        }

        float colormap_blue(float x) {
            if (x < 0.0) {
                return 54.0 / 255.0;
            } else if (x < 7249.0 / 82979.0) {
                return (829.79 * x + 54.51) / 255.0;
            } else if (x < 20049.0 / 82979.0) {
                return 127.0 / 255.0;
            } else if (x < 327013.0 / 810990.0) {
                return (792.02249341361393720147485376583 * x - 64.364790735602331034989206222672) / 255.0;
            } else {
                return 1.0;
            }
        }

        float4 colormap(float x) {
            return float4(colormap_red(x) * _rModifire, colormap_green(x) * _gModifire, colormap_blue(x) * _bModifire, 1.0);
        }

        // https://iquilezles.org/articles/warp
        /*float noise( in float2 x )
        {
            float2 p = floor(x);
            float2 f = fract(x);
            f = f*f*(3.0-2.0*f);
            float a = textureLod(iChannel0,(p+float2(0.5,0.5))/256.0,0.0).x;
	        float b = textureLod(iChannel0,(p+float2(1.5,0.5))/256.0,0.0).x;
	        float c = textureLod(iChannel0,(p+float2(0.5,1.5))/256.0,0.0).x;
	        float d = textureLod(iChannel0,(p+float2(1.5,1.5))/256.0,0.0).x;
            return mix(mix( a, b,f.x), mix( c, d,f.x),f.y);
        }*/


        float rand(float2 n) { 
            return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
        }

        float noise(float2 p){
            float2 ip = floor(p);
            float2 u = frac(p);
            u = u*u*(3.0-2.0*u);

            float res = lerp(
                lerp(rand(ip),rand(ip+float2(1.0,0.0)),u.x),
                lerp(rand(ip+float2(0.0,1.0)),rand(ip+float2(1.0,1.0)),u.x),u.y);
            return res*res;
        }

        float fbm( float2 p )
        {
            float f = 0.0;

            f += 0.500000*noise( p + _Time  ); p = p*2.02;
            f += 0.031250*noise( p ); p = p*2.01;
            f += 0.250000*noise( p ); p = p*2.03;
            f += 0.125000*noise( p ); p = p*2.01;
            f += 0.062500*noise( p ); p = p*2.04;
            f += 0.015625*noise( p + sin(_Time) );

            return f/0.96875;
        }

        float pattern( in float2 p )
        {
	        return fbm( p + fbm( p + fbm( p ) ) );
        }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            
                    float2 uv = i.uv;
	                float shade = pattern(uv);
                    
                return float4(colormap(shade).rgb, shade);;
            }
            ENDCG
        }
    }
}
