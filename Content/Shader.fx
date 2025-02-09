//u do not have to understand what is happening in these first 10 lines
//but essentially we tell the compiler to use DirectX if we are on windows otherwise use OpenGL!
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


//here we define the uniforms!

//that matrix
matrix WorldViewProjection;

//these 2 are automatically passed in by the Spritbatch, which are the textures ( dedodatedwam.png in our case :3 )
Texture2D SpriteTexture;
sampler2D SpriteTextureSampler;

//the brightness uniform
float brightness;

//the time uniform
float time;

//u dont have to give a f**k from here all the way toooo
// |
// |
// V
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};
// |
// |
// V
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};
// |
// |
// V
VertexShaderOutput MainVS(in VertexShaderInput input) // this is the Vertex Shader By the way which we wont dive into cuz it's useless for us LOL
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.Position = mul(input.Position, WorldViewProjection);
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}
// |
// |
// V
//HERE! now the fun begins

//This is the Fragment Shader Code!
float4 MainPS(VertexShaderOutput input) : COLOR
{
    //we create a vec2 holding the pixel coordinates normalized (0 -> 1)   (in glsl vec2 or vec3 etc are float2 float3 etc in HLSL!)
    float2 texCoord = input.TextureCoordinates;
    
    // we modify where the pixel is gonna fall on the screen through this sin function
    //if you know your math you'll understand 🧐
    texCoord.x += sin(texCoord.y * 20 + time * 0.001) * 0.02;
    
    //next we use that distorted coord to extract the color, we use tex2D
    float4 color = tex2D(SpriteTextureSampler, texCoord);
    //SpriteTextureSampler represents the texture where we will take the color
    //texCoord is where in that texture the color is gonna be extracted!
    
    //here we multiply the red/green/blue channels by the brightness to make it either brighter or darker!
    color.rgb *= brightness;
    
    //we output the FINAL color!
    return  color;
}

//this is what tells the SpriteBatch which vertex/fragment shader to use!
//you'll mostly not use it!
technique BasicColorDrawing
{
	pass P0
	{
        //we tell it to use MainVS as vertex shader
		VertexShader = compile VS_SHADERMODEL MainVS();

        //and MainPS as frag shader
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};