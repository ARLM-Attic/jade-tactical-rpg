using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FlatRedBall;
using FlatRedBall.Graphics.PostProcessing;

namespace FRBDemo.PostProcessing
{
    #region XML Docs
    /// <summary>
    /// A depth of field post-processing effect
    /// </summary>
    #endregion
    public class DepthOfField : PostProcessingEffectBase
    {
        #region Fields

        #region Blur Fields

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

        #endregion

        #region Depth of Field Fields

        // The distance at which the image is in focus
        private float mFocalDistance = 20f;

        // The range around that distance in which the focus drops off
        private float mFocalLength = 20f;

        #endregion

        #endregion

        #region Properties

        #region XML Docs
        /// <summary>
        /// Controls the strength of the Gaussian blur
        /// (the standard deviation of the normal curve used for sampling nearby coordinates)
        /// </summary>
        #endregion
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

        #region XML Docs
        /// <summary>
        /// Controls the strength of the linear blur
        /// (Scales the sampling points to nearer or farther away)
        /// </summary>
        #endregion
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

        #region XML Docs
        /// <summary>
        /// The distance at which the focal point is set
        /// </summary>
        #endregion
        public float FocalDistance
        {
            get { return mFocalDistance; }
            set { mFocalDistance = value; }
        }

        #region XML Docs
        /// <summary>
        /// The length of focus (distance between completely focused and
        /// completely unfocused)
        /// </summary>
        #endregion
        public float FocalLength
        {
            get { return mFocalLength; }
            set { mFocalLength = value; }
        }

        #endregion

        #region Methods

        #region Constructor

        #region XML Docs
        /// <summary>
        /// Load the effect, calling the base constructor with the effect path specified
        /// </summary>
        #endregion
        public DepthOfField()
            : base(@"Dof")
        {
        }

        #region XML Docs
        /// <summary>
        /// Initializes the effect, enables it, and sets up sampling parameters
        /// </summary>
        #endregion
        public override void InitializeEffect()
        {
            base.InitializeEffect();

            // Enable the effect (default is disabled)
            mEnabled = true;

            // Set up the sampling parameters for the blur portion of the effect
            SetSampleParameters();
        }

        #endregion

        #region XML Docs
        /// <summary>
        /// Calculates and sets the blur sampling parameters in the shader
        /// A gaussian blur is used
        /// </summary>
        #endregion
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
                mSampleOffsetsHorizontal[i] = (new Vector2(
                    (float)(i - sampleMid) * 2.0f * mSampleScale + 0.5f, 0.5f)) *
                    PostProcessingManager.PixelSize;
                mSampleOffsetsVertical[i] = (new Vector2(
                    0.5f, (float)(i - sampleMid) * 2.0f * mSampleScale + 0.5f)) *
                    PostProcessingManager.PixelSize;
            }

            #endregion

            // Normalize sample weights
            for (int i = 0; i < mSampleWeights.Length; i++)
            {
                mSampleWeights[i] /= weightSum;
            }
        }

        /// <summary>
        /// Sets the shader parameters for this effect just before rendering it
        /// </summary>
        /// <param name="camera">The camera that is passed from the renderer</param>
        protected override void SetEffectParameters(Camera camera)
        {
            // Set parameters
            mEffect.Parameters["sampleWeights"].SetValue(mSampleWeights);
            mEffect.Parameters["focalDistance"].SetValue(mFocalDistance);
            mEffect.Parameters["focalLength"].SetValue(mFocalLength);

            // Set the depth texture in the shader from the camera's depth texture
            // Note: This is setting the texture directly on the graphics device.
            //       You may also specify a texture and sampler in the shader and
            //       set those.            
            FlatRedBallServices.GraphicsDevice.Textures[1] =
                camera.GetRenderTexture(FlatRedBall.Graphics.RenderMode.Depth);
        }

        #region XML Docs
        /// <summary>
        /// Draws the effect, storing the final result in mTexture (which becomes the
        /// input for the next post-processing effect).
        /// </summary>
        /// <param name="camera">The camera currently drawing</param>
        /// <param name="screenTexture">The current screen texture, as results from previous
        ///                             post-processing effects</param>
        /// <param name="baseRectangle">The rectangle on the screen to draw to</param>
        /// <param name="clearColor">The current background color</param>
        #endregion
        public override void Draw(
            Camera camera,
            ref Texture2D screenTexture,
            ref Rectangle baseRectangle,
            Color clearColor)
        {
            // Set the effect parameters (this must be called)
            SetEffectParameters(camera);

            #region Create the Blur Buffer

            // Set the effect technique
            mEffect.CurrentTechnique = mEffect.Techniques["Blur"];

            // Create a texture to store blur information
            Texture2D blurTexture = null;

            #region Horizontal Blur

            // Set horizontal sampling offsets
            mEffect.Parameters["sampleOffsets"].SetValue(mSampleOffsetsHorizontal);

            // Draw the horizontal pass
            // Start with screenTexture as the source, and output to blurTexture
            DrawToTexture(camera, ref blurTexture, clearColor, ref screenTexture, ref baseRectangle);

            #endregion

            #region Vertical Blur

            // Set vertical sampling offsets
            mEffect.Parameters["sampleOffsets"].SetValue(mSampleOffsetsVertical);

            // Draw the vertical pass
            // Start with blurTexture as the source (blur in the other direction now), and
            // output to blurTexture (overwrite the old texture)
            DrawToTexture(camera, ref blurTexture, clearColor, ref blurTexture, ref baseRectangle);

            #endregion

            #endregion

            #region Depth of Field

            // Set blur texture on the graphics device
            FlatRedBallServices.GraphicsDevice.Textures[2] = blurTexture;

            // Set the effect technique
            mEffect.CurrentTechnique = mEffect.Techniques["DepthOfField"];

            // Draw the depth of field pass
            // Start with the screenTexture, output to mTexture (last pass MUST output to mTexture -
            // it will become the screenTexture on the next post-processing effect)
            DrawToTexture(camera, ref mTexture, clearColor, ref screenTexture, ref baseRectangle);

            #endregion
        }

        #endregion
    }
}
