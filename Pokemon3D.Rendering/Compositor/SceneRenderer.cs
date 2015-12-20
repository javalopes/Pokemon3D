namespace Pokemon3D.Rendering.Compositor
{
    public interface SceneRenderer
    {
        bool EnablePostProcessing { get; set; }

        void AddPostProcessingStep(PostProcessingStep step);

        void Draw(Scene scene);

         RenderSettings RenderSettings { get; }
    }
}
