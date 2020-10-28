float4 _CameraColorTexture_TexelSize;

void Outline_float(float2 UV, Texture2D ObjectTexture, SamplerState ObjectSampler, out float4 Color)
{
    float OutlineThickness = 1.0f;
    float halfScaleFloor = floor(OutlineThickness * 0.5);
    float halfScaleCeil = ceil(OutlineThickness * 0.5);
    float2 Texel = (1.0) / float2(_CameraColorTexture_TexelSize.z, _CameraColorTexture_TexelSize.w);

    float2 uvSamples[4];
    float2 texel = float2(2, 2);
    uvSamples[0] = UV - float2(Texel.x, Texel.y) * halfScaleFloor;
    uvSamples[1] = UV + float2(Texel.x, Texel.y) * halfScaleCeil;
    uvSamples[2] = UV + float2(Texel.x * halfScaleCeil, -Texel.y * halfScaleFloor);
    uvSamples[3] = UV + float2(-Texel.x * halfScaleFloor, Texel.y * halfScaleCeil);

    float4 colorSamples[4];
    for (int i = 0; i < 4; i++)
        colorSamples[i] = ObjectTexture.Sample(ObjectSampler, uvSamples[i]);

    float4 colorFiniteDifference0 = colorSamples[1] - colorSamples[0];
    float4 colorFiniteDifference1 = colorSamples[3] - colorSamples[2];
    float edge = sqrt(dot(colorFiniteDifference0, colorFiniteDifference0) + dot(colorFiniteDifference1, colorFiniteDifference1));
    Color = edge;
}