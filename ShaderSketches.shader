Shader "ShaderSketches/Template" {
	Properties {
		_MainTex ("MainTexture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Glossiness("Smoothness", Range(0, 1)) = 0.5
		_Metallic("Metallic", Range(0, 1)) = 0.0
	}

		
	CGINCLUDE
	#include"UnityCG.cginc"

	#define PI 3.14159265359

	float rand(float2 st) {
		return frac(sin(dot(st, float2(12.9898, 78.233))) * 43758.5453);
	}

	float noise(fixed2 st) {
		fixed2 p = floor(st);
		return p;
	}

	float valueNoise(fixed2 st) {
		fixed2 p = floor(st);
		fixed2 f = frac(st);

		float v00 = rand(p + fixed2(0, 0));
		float v10 = rand(p + fixed2(1, 0));
		float v01 = rand(p + fixed2(0, 1));
		float v11 = rand(p + fixed2(1, 1));

		fixed2 u = f * f * (3.0 - 2.0 * f);
		float v0010 = lerp(v00, v10, u.x);
		float v0111 = lerp(v01, v11, u.x);
		return lerp(v0010, v0111, u.y);
	}

	float circle(float2 st) {
		return step(0.3, distance(0.5, st));
	}

	float box(float2 st, float size) {
		size = 0.5 + size * 0.5;
		st = step(st, size) * step(1.0 - st, size);
		return st.x * st.y;
	}

	float wave(float2 st, float n) {
		st = (floor(st * n) + 0.5) / n;
		float d = distance(0.5, st);
		return (1 + sin(d * 3 - _Time.y * 3)) * 0.5;
	}

	float box_wave(float2 uv, float n) {
		float2 st = frac(uv * n);
		float size = wave(uv, n);
		return box(st, size);
	}

	float box_size(float2 st, float n) {
		st = (floor(st * n) + 0.5) / n;
		float offs = rand(st) * 5;
		return(1 + sin(_Time.y * 3 + offs)) * 0.5;
	}

	fixed4 frag(v2f_img i) :SV_Target{

		float c = valueNoise(box_wave(i.uv, 10));
		return fixed4(c , c, c, 0);


		/*座標をそのまま色に変えてる
		return float4(i.uv.x, i.uv.y, 0, 1);
		*/

		/*桜のはなびら模様
		float2 st = 0.5 - i.uv;
		float a = atan2(st.y, st.x);

		float r = length(st);
		float d = min(abs(cos(a * 2.5)) + 0.4, abs(sin(a * 2.5)) + 1.1) * 0.32;

		float4 color = lerp(0.8, float4(0, 0.4, 1, 1), i.uv.y);

		float petal = step(r, d);
		color = lerp(color, lerp(float4(1, 0.3, 1, 1), 1, r * 2.5), petal);

		float cap = step(distance(0, st), 0.07);
		color = lerp(color, float4(0.99, 0.78, 0, 1), cap);

		return color;
		*/

		/*バリューノイズにボックスウェイブをいれてみた
		float c = valueNoise(box_wave(i.uv, 10));
		return fixed4(c , c, c, 1);
		*/

		/*バリューノイズ
		float c = valueNoise(i.uv * 8);
		return fixed4(c, c, c, 1);
		*/

		/*ランダムノイズ（ブラウン管の砂嵐的な）
		float c = rand(sin(i.uv * _Time.y * 20));
		return float4(c , c, c, 1);
		*/

		/*同心円　広がってく
		float d = distance(float2(0.5, 0.5), i.uv);
		//d = d * 20;
		//d = abs(sin(d));
		d = step(d, abs(sin(d * 10 - _Time.y * 5)));
		return d;
		*/

		/*同心円
		float d = distance(float2(0.5, 0.5), i.uv);
		d = d * 30;
		d = abs(sin(d));
		d = step(0.5, d);
		return d;
		*/

		/*円をもとにした歪み
		float x = 2 * i.uv.y + sin(_Time.y * 0.5);
		float distort = sin(_Time.y * 10.0) * 0.1 * sin(5.0 * x) * (-(x - 1) * (x - 1) + 1);
		i.uv.x += distort;
		return float4(circle(i.uv - float2(0, distort)*0.2),
			circle(i.uv + float2(0, distort)*0.3),
			circle(i.uv + float2(distort, 0)*0.4),
			1);
			*/

		/*ｎ個の四角がバラバラに拡縮
		float n = 3;
		float2 st = frac(i.uv * n);
		float size = box_size(i.uv, n);
		return box(st, size);
		*/
	}


	ENDCG

	SubShader {
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
