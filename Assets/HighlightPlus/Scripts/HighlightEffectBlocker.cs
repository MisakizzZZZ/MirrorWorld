using UnityEngine;

namespace HighlightPlus {

    [ExecuteInEditMode]
    [DefaultExecutionOrder(-100)]
    public class HighlightEffectBlocker : MonoBehaviour {

        MeshFilter mf;
        public bool blockOutlineAndGlow;
        public bool blockOverlay;

        Material blockerOutlineAndGlowMat;
        Material blockerOverlayMat;
        Material blockerAllMat;

        void OnEnable () {
            mf = GetComponentInChildren<MeshFilter>();
            if (blockerOutlineAndGlowMat == null) {
                blockerOutlineAndGlowMat = Resources.Load<Material>("HighlightPlus/HighlightBlockerOutlineAndGlow");
            }
            if (blockerOverlayMat == null) {
                blockerOverlayMat = Resources.Load<Material>("HighlightPlus/HighlightBlockerOverlay");
            }
            if (blockerAllMat == null) {
                blockerAllMat = Resources.Load<Material>("HighlightPlus/HighlightUIMask");
            }
        }


        void Update () {
            int stencilID = 0;
            if (blockOutlineAndGlow) {
                stencilID = 2;
            }
            if (blockOverlay) {
                stencilID += 4;
            }
            if (stencilID == 0 || mf == null || mf.sharedMesh == null) return;

            if (stencilID == 2) {
                Graphics.DrawMesh(mf.sharedMesh, mf.transform.localToWorldMatrix, blockerOutlineAndGlowMat, gameObject.layer);
            }
            else if (stencilID == 4) {
                Graphics.DrawMesh(mf.sharedMesh, mf.transform.localToWorldMatrix, blockerOverlayMat, gameObject.layer);
            }
            else if (stencilID == 6) {
                Graphics.DrawMesh(mf.sharedMesh, mf.transform.localToWorldMatrix, blockerAllMat, gameObject.layer);
            }
        }
    }
}
