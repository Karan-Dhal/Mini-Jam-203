using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blit : ScriptableRendererFeature
{

    public class BlitPass : ScriptableRenderPass
    {
        public enum RenderTarget
        {
            Color,
            RenderTexture,
        }

        public Material blitMaterial = null;
        public int blitShaderPassIndex = 0;
        public FilterMode filterMode { get; set; }

        private RTHandle source { get; set; }
        private RTHandle destination { get; set; }

        RTHandle m_TemporaryColorTexture;
        string m_ProfilerTag;

        public BlitPass(RenderPassEvent renderPassEvent, Material blitMaterial, int blitShaderPassIndex, string tag)
        {
            this.renderPassEvent = renderPassEvent;
            this.blitMaterial = blitMaterial;
            this.blitShaderPassIndex = blitShaderPassIndex;
            m_ProfilerTag = tag;
        }

        public void Setup(RTHandle source, RTHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            // Can't read and write to same color target, use a TemporaryRT
            if (destination == renderingData.cameraData.renderer.cameraColorTargetHandle)
            {
                RenderingUtils.ReAllocateIfNeeded(ref m_TemporaryColorTexture, opaqueDesc, filterMode, TextureWrapMode.Clamp, name: "_TemporaryColorTexture");
                Blitter.BlitCameraTexture(cmd, source, m_TemporaryColorTexture, blitMaterial, blitShaderPassIndex);
                Blitter.BlitCameraTexture(cmd, m_TemporaryColorTexture, source);
            }
            else
            {
                Blitter.BlitCameraTexture(cmd, source, destination, blitMaterial, blitShaderPassIndex);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            m_TemporaryColorTexture?.Release();
        }

        public void Dispose()
        {
            m_TemporaryColorTexture?.Release();
        }
    }

    [System.Serializable]
    public class BlitSettings
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

        public Material blitMaterial = null;
        public int blitMaterialPassIndex = 0;
        public Target destination = Target.Color;
        public string textureId = "_BlitPassTexture";
    }

    public enum Target
    {
        Color,
        Texture
    }

    public BlitSettings settings = new BlitSettings();
    RTHandle m_RenderTextureHandle;

    BlitPass blitPass;

    public override void Create()
    {
        var passIndex = settings.blitMaterial != null ? settings.blitMaterial.passCount - 1 : 1;
        settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
        blitPass = new BlitPass(settings.Event, settings.blitMaterial, settings.blitMaterialPassIndex, name);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTargetHandle;
        var dest = (settings.destination == Target.Color) ? renderer.cameraColorTargetHandle : m_RenderTextureHandle;

        if (settings.blitMaterial == null)
        {
            Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            return;
        }

        if (settings.destination == Target.Texture)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref m_RenderTextureHandle, desc, FilterMode.Point, TextureWrapMode.Clamp, name: settings.textureId);
        }

        blitPass.Setup(src, dest);
        renderer.EnqueuePass(blitPass);
    }

    protected override void Dispose(bool disposing)
    {
        blitPass?.Dispose();
        m_RenderTextureHandle?.Release();
    }
}