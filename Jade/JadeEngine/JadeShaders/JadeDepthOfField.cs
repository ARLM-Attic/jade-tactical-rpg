using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JadeEngine.JadeShaders
{
    public class JadeDepthOfField : JadePostProcessor
    {
        // The number of sample taps per pixel
        private int mBlurSampleCount = 7;
        // Blur parameters (standard deviation for normal distribution, and linear scaling)
        // This controls the out-of-focus areas of the image
        private float mBlurStandardDeviation = 4.0f;
        private float mSampleScale = 1.0f;
        // Sampling positions
        private float[] mSampleWeights;
        private Vector2[] mSampleOffsetsHorizontal;
        private Vector2[] mSampleOffsetsVertical;
        // The distance at which the image is in focus
        private float mFocalDistance = 20f;
        // The range around that distance in which the focus drops off
        private float mFocalLength = 20f;

        public float GaussianStrength
        {
            get { return mBlurStandardDeviation; }
            set
            {
                if (mBlurStandardDeviation != value)
                {
                    mBlurStandardDeviation = value;
                    SetSampleParameters();
                }
            }
        }
        public float LinearStrength
        {
            get { return mSampleScale; }
            set
            {
                if (mSampleScale != value)
                {
                    mSampleScale = value;
                    SetSampleParameters();
                }
            }
        }
        public float FocalDistance
        {
            get { return mFocalDistance; }
            set { mFocalDistance = value; }
        }
        public float FocalLength
        {
            get { return mFocalLength; }
            set { mFocalLength = value; }
        }

        public JadeDepthOfField() : base(@"Content/Shaders/Dof")
        {
            SetSampleParameters();
        }

        internal void SetSampleParameters()
        {
            // Local variables
            int sampleMid = (mBlurSampleCount / 2);
            mSampleWeights = new float[mBlurSampleCount];
            mSampleOffsetsHorizontal = new Vector2[mBlurSampleCount];
            mSampleOffsetsVertical = new Vector2[mBlurSampleCount];

            #region Calculate Sampling Weights and Offsets

            // Calculate values using normal (gaussian) distribution
            float weightSum = 0f;
            for (int i = 0; i < mBlurSampleCount; i++)
            {
                // Get weight
                mSampleWeights[i] =
                    1f / (((float)System.Math.Sqrt(2.0 * System.Math.PI) / mBlurStandardDeviation) *
                    (float)System.Math.Pow(System.Math.E,
                        System.Math.Pow((double)(i - sampleMid), 2.0) /
                        (2.0 * System.Math.Pow((double)mBlurStandardDeviation, 2.0))));

                // Add to total weight value (for normalization)
                weightSum += mSampleWeights[i];

                // Get offsets
                mSampleOffsetsHorizontal[i] = (new Vector2( (float) (i - sampleMid)*2.0f*mSampleScale + 0.5f, 0.5f))*1; //PostProcessingManager.PixelSize;
                mSampleOffsetsVertical[i] = (new Vector2(0.5f, (float) (i - sampleMid)*2.0f*mSampleScale + 0.5f))*1; //PostProcessingManager.PixelSize;
            }

            #endregion

            // Normalize sample weights
            for (int i = 0; i < mSampleWeights.Length; i++)
                mSampleWeights[i] /= weightSum;
        }

        internal override void SetParameters(JadeEngine.JadeObjects.JadeObject obj)
        {
            if (Effect.Parameters["sampleWeights"] != null) Effect.Parameters["sampleWeights"].SetValue(mSampleWeights);
            if (Effect.Parameters["focalDistance"] != null) Effect.Parameters["focalDistance"].SetValue(mFocalDistance);
            if (Effect.Parameters["focalLength"] != null) Effect.Parameters["focalLength"].SetValue(mFocalLength);
        }

        internal void Draw(GraphicsDevice gd)
        {
            // Set the effect technique
            Effect.CurrentTechnique = Effect.Techniques["Blur"];

            // Set horizontal sampling offsets
            Effect.Parameters["sampleOffsets"].SetValue(mSampleOffsetsHorizontal);

            base.Draw(gd);

            // Set vertical sampling offsets
            Effect.Parameters["sampleOffsets"].SetValue(mSampleOffsetsVertical);

            base.Draw(gd);

            // Set the effect technique
            Effect.CurrentTechnique = Effect.Techniques["DepthOfField"];

            base.Draw(gd);
        }
    }
}
