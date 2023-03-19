Shader "Custom/BGShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
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



float random (in float2 _st) {
    return frac(sin(dot(_st.xy,
                    float2(12.9898, 78.233)))*
                 43758.5453123);
}

float random1 (float f) {
    return random(float2(f, -0.128));
}

float2 random2( float2 p ) {
    return frac(sin(float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3))))*43758.5453);
}

// Commutative smooth minimum function. Provided by Tomkh, and taken 
// from Alex Evans's (aka Statix) talk:
float smin(float a, float b, float k){
   float f = max(0., 1. - abs(b - a)/k);
   return min(a, b) - k*.25*f*f;
}

float noise(float s) {  
    float i = floor(s);
    float f = frac(s);
    float n = lerp(random(float2(i, 0.)), 
                  random(float2(i+1., 0.)), 
                  smoothstep(0.0, 1., f)); 
   
    return n;
}

float3 rgb2hsv(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float map(float value, float inMin, float inMax, float outMin, float outMax) {

  return outMin + (outMax - outMin) * (value - inMin) / (inMax - inMin);

} 



float2 map(float2 value, float2 inMin, float2 inMax, float2 outMin, float2 outMax) {

  return outMin + (outMax - outMin) * (value - inMin) / (inMax - inMin);

}



float3 map(float3 value, float3 inMin, float3 inMax, float3 outMin, float3 outMax) {

  return outMin + (outMax - outMin) * (value - inMin) / (inMax - inMin);

}



float4 map(float4 value, float4 inMin, float4 inMax, float4 outMin, float4 outMax) {
  return outMin + (outMax - outMin) * (value - inMin) / (inMax - inMin);
}

float sin01(float n) {
    return sin(n)/2.+.5;
}


float4 blend(float4 bg, float4 fg) {
    float4 c = float4(0.,0.,0.,0.);
    c.a = 1.0 - (1.0 - fg.a) * (1.0 - bg.a);
    if(c.a < .00000) return c;
    
    c.r = fg.r * fg.a / c.a + bg.r * bg.a * (1.0 - fg.a) / c.a;
    c.g = fg.g * fg.a / c.a + bg.g * bg.a * (1.0 - fg.a) / c.a;
    c.b = fg.b * fg.a / c.a + bg.b * bg.a * (1.0 - fg.a) / c.a;
    
    return c;
}

// Some useful functions
float3 mod289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
float2 mod289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
float3 permute(float3 x) { return mod289(((x*34.0)+1.0)*x); }

//
// Description : GLSL 2D simplex noise function
//      Author : Ian McEwan, Ashima Arts
//  Maintainer : ijm
//     Lastmod : 20110822 (ijm)
//     License :
//  Copyright (C) 2011 Ashima Arts. All rights reserved.
//  Distributed under the MIT License. See LICENSE file.
//  https://github.com/ashima/webgl-noise
//
float snoise(float2 v) {

    // Precompute values for skewed triangular grid
    const float4 C = float4(0.211324865405187,
                        // (3.0-sqrt(3.0))/6.0
                        0.366025403784439,
                        // 0.5*(sqrt(3.0)-1.0)
                        -0.577350269189626,
                        // -1.0 + 2.0 * C.x
                        0.024390243902439);
                        // 1.0 / 41.0

    // First corner (x0)
    float2 i  = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);

    // Other two corners (x1, x2)
    float2 i1 = float2(0.0,0.);
    i1 = (x0.x > x0.y)? float2(1.0, 0.0):float2(0.0, 1.0);
    float2 x1 = x0.xy + C.xx - i1;
    float2 x2 = x0.xy + C.zz;

    // Do some permutations to avoid
    // truncation effects in permutation
    i = mod289(i);
    float3 p = permute(
            permute( i.y + float3(0.0, i1.y, 1.0))
                + i.x + float3(0.0, i1.x, 1.0 ));

    float3 m = max(0.5 - float3(
                        dot(x0,x0),
                        dot(x1,x1),
                        dot(x2,x2)
                        ), 0.0);
                        
                        m = m*m ;
    m = m*m ;

    // Gradients:
    //  41 pts uniformly over a line, mapped onto a diamond
    //  The ring size 17*17 = 289 is close to a multiple
    //      of 41 (41*7 = 287)

    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

    // Normalise gradients implicitly by scaling m
    // Approximation of: m *= inversesqrt(a0*a0 + h*h);
    m *= 1.79284291400159 - 0.85373472095314 * (a0*a0+h*h);

    // Compute final noise value at P
    float3 g = float3(0.0,0.0,0.0);
    g.x  = a0.x  * x0.x  + h.x  * x0.y;
    g.yz = a0.yz * float2(x1.x,x2.x) + h.yz * float2(x1.y,x2.y);
    return dot(m, g);
}


float fbm(float2 x, float amplitude, float frequency, float offset) {
    x += offset;
    float y = 0.;
    // Properties
    const int octaves = 8;
    float lacunarity = 0.;
    float gain = 0.;
    
    // Initial values
    //sin(u_time) * 5. + 10.;
    //sin(u_time/10. + 10.);
    
    // Loop of octaves
    for (int i = 0; i < octaves; i++) {
        y += amplitude * snoise(frequency*x);
        frequency *= lacunarity;
        amplitude *= gain;
    }
    
    return y;
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

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
            
                    float4 color = float4(-1.1, -1.1, 1.1, 1.);


            for(float k=0.; k<2.; k++) {
                float2 st = (i.uv - .5) -1;
                float2 uv = st;
        
                st *=2.788;
        

                // Tile
                float2 i_st = floor(st);
                float2 f_st = frac(st);

                float m_dist = 0.5; // min distance

                for(int j=-3; j<=3; j++) {
                    for(int i=-3; i<=3; i++) {
                        float2 neighbor = float2(float(i), float(j));
                        float2 offset = random2(i_st + neighbor);

                        offset = 0.5 + 0.5 * sin(_Time/1.5 + 6.2831 * offset );
                      //  offset = (iMouse - .5 * iResolution.xy) / iResolution.y * 2. * offset;
                       // offset += sin(iTime/2. + 6.2831 * offset);

                        float2 pos = neighbor + offset - f_st;
                        float dist = length(pos);

                        // Metaball
                        float diff = k/2. + 0.084;
                        diff = k/9.120;
                        m_dist = smin(m_dist, dist, 1.640 + diff);            
                    }
                }


                float f = m_dist;
                f *= 5.;
                #define steps 4.
                f = ceil(f *steps) / steps;
                f = map(f, -3., 0., 1., 0.000);

                
                float incr = (1./(steps*3.));

                // Map colors to height
               for(float q = 0.; q<steps*3.; q++) {

              // Get the current height
                    float fc = smoothstep(q * incr, q*incr+-0.062, f);
                    fc = step(q * incr, f);
            
              // Base color
                    float h =  map(q*incr, 0., 1., 0.160, 0.844);
              h +=  + fc + k/3.392;
                   float co = sin01(_Time);
                   co = map(co, 0., 1., .5, .2);
                   h += uv.x * uv.y + co;
           
                    // Blend it
                    float4 c = float4(h, 0.864, 1., 0.0);
                   c = float4(hsv2rgb(c.xyz), 0.444);
                    color = blend(color, c * fc);
                }
        

            }
    
            float p = 0.;
            float2 st = (i.uv - .5);

            p = fbm(st, 100., .7, _Time);
            p = map(p, 0., 0.432, 0.664, 1.040);
           // color = float4(color.xyz * p, 1.);
            color.r = 0.;
    
    return color;
                    
                
            }
            ENDCG
        }
    }
}
