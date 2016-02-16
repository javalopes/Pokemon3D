using Pokemon3D.Editor.Core.Model;

namespace Pokemon3D.Editor.Core.DetailViewModels
{
    class ModelDetailViewModel : DetailViewModel
    {
        private ModelModel _model;
        private PlatformService _platformService;

        public ModelDetailViewModel(PlatformService platformServce, ModelModel model)
        {
            _model = model;
            _platformService = platformServce;
        }

        internal override void OnActivate()
        {
            _platformService.Activate3DViewForModel(_model.FilePath);
        }

        internal override void OnDeactivate()
        {
            _platformService.Deactivate3DView();
        }
    }
}
