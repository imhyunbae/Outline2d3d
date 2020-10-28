using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderPassFeature : ScriptableRendererFeature
{
    class OutlineRenderPass : ScriptableRenderPass
    {
        public Material material;
        public RenderTargetIdentifier renderTargetIdentifier;
        RenderTargetHandle tempTexture;
        public RenderTexture objectTexture;
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.camera.clearFlags == CameraClearFlags.Skybox) // only when rendering camera
            {
                material.SetTexture("_ObjectTex", objectTexture);
                CommandBuffer cmd = CommandBufferPool.Get("outline");
                cmd.Clear();

                cmd.Blit(renderTargetIdentifier, tempTexture.Identifier(), material, 0);
                cmd.Blit(tempTexture.Identifier(), renderTargetIdentifier);

                context.ExecuteCommandBuffer(cmd);

                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    OutlineRenderPass outlineRenderPass;
    public RenderPassEvent renderPassEvent;
    public Material material;
    // public RenderTexture objectTexture;

    public override void Create()
    {        
        outlineRenderPass = new OutlineRenderPass{material=material};
        outlineRenderPass.renderPassEvent = renderPassEvent;
        var outlineCamera = Camera.main.GetComponent<OutlineCamera>();
        if (outlineCamera != null)
            outlineRenderPass.objectTexture = outlineCamera.outlineMap;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        outlineRenderPass.renderTargetIdentifier = renderer.cameraColorTarget;
        renderer.EnqueuePass(outlineRenderPass);
    }
}