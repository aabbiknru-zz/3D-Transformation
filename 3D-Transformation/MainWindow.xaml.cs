using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D_Transformation
{
    public partial class MainWindow : Window
    {
        Model3DGroup translationModelGroup = new Model3DGroup();
        Model3DGroup scalingModelGroup = new Model3DGroup();

        private int[,] TrX = new int[4, 8];
        private int[,] TrF = new int[4, 4];
        private int[,] TrY = new int[4, 8];

        private int[,] ScX = new int[4, 8];
        private int[,] ScF = new int[4, 4];
        private int[,] ScY = new int[4, 8];

        public MainWindow()
        {
            InitializeComponent();

            SetZeroMatrix(TrX);
            SetZeroMatrix(TrF);
            SetZeroMatrix(TrY);
            TrObjectList();
            TrDisableAllInput();
            TrDrawCartesianAxis();

            SetZeroMatrix(ScX);
            SetZeroMatrix(ScF);
            SetZeroMatrix(ScY);
            ScObjectList();
            ScDisableAllInput();
            ScDrawCartesianAxis();
        }

        private void SetZeroMatrix(int[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    matrix[row, col] = 0;
                }
            }
        }

        private void MultiplyMatrix(int[,] resultMatrix, int[,]firstMatrix, int[,]secondMatrix)
        {
            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < resultMatrix.GetLength(0); k++)
                    {
                        resultMatrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                    }
                }
            }
        }

        //  _______ _____            _   _  _____ _            _______ _____ ____  _   _ 
        // |__   __|  __ \     /\   | \ | |/ ____| |        /\|__   __|_   _/ __ \| \ | |
        //    | |  | |__) |   /  \  |  \| | (___ | |       /  \  | |    | || |  | |  \| |
        //    | |  |  _  /   / /\ \ | . ` |\___ \| |      / /\ \ | |    | || |  | | . ` |
        //    | |  | | \ \  / ____ \| |\  |____) | |____ / ____ \| |   _| || |__| | |\  |
        //    |_|  |_|  \_\/_/    \_\_| \_|_____/|______/_/    \_\_|  |_____\____/|_| \_|        

        private void TrDrawCartesianAxis()
        {
            var axisBuilder = new MeshBuilder(true, true, true);
            axisBuilder.AddPipe(new Point3D(-100, 0, 0), new Point3D(100, 0, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, -100, 0), new Point3D(0, 100, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, 0, -100), new Point3D(0, 0, 100), 0, 0.1, 360);

            var mesh = axisBuilder.ToMesh(true);
            var material = MaterialHelper.CreateMaterial(Colors.Gray);
            translationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = material
            });
            TrModelVisual.Content = translationModelGroup;
        }

        public void TrDrawObjectOrigin()
        {
            var meshBuilder = new MeshBuilder(false, false);            
            meshBuilder.AddBox(new Rect3D(TrX[0, 0], TrX[1, 0], TrX[2, 0], Int32.Parse(txtTrObjectLength.Text), Int32.Parse(txtTrObjectWidth.Text), Int32.Parse(txtTrObjectHeight.Text)));

            var mesh = meshBuilder.ToMesh(true);
            
            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);

            translationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = blueMaterial
            });
        }

        public void TrDrawObjectDestination()
        {
            var meshBuilder = new MeshBuilder(false, false);            
            meshBuilder.AddBox(new Rect3D(TrY[0, 0], TrY[1, 0], TrY[2, 0], Int32.Parse(txtTrObjectLength.Text), Int32.Parse(txtTrObjectWidth.Text), Int32.Parse(txtTrObjectHeight.Text)));

            var mesh = meshBuilder.ToMesh(true);

            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);            

            translationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = redMaterial
            });
        }

        private void TrObjectList()
        {
            cbxTrObject.Items.Add("Persegi Panjang");
            cbxTrObject.Items.Add("Balok");
        }

        private void TrDisableAllInput()
        {
            txtTrObjectLength.IsEnabled = false;
            txtTrObjectWidth.IsEnabled = false;
            txtTrObjectHeight.IsEnabled = false;
            btnTrSetObject.IsEnabled = false;

            txtTrOriginX.IsEnabled = false;
            txtTrOriginY.IsEnabled = false;
            txtTrOriginZ.IsEnabled = false;
            btnTrSetOrigin.IsEnabled = false;

            txtTrMovementX.IsEnabled = false;
            txtTrMovementY.IsEnabled = false;
            txtTrMovementZ.IsEnabled = false;
            btnSetTranslation.IsEnabled = false;

            btnTranslate.IsEnabled = false;
        }

        private void TrLineSeparator()
        {
            txtTrLine.Text = String.Empty;

            if (cbxTrObject.SelectedIndex == 0)
            {
                for (int i = 0; i < txtTranslateDescription.Text.Length; i++)
                {
                    if (i == 0 || i == txtTranslateDescription.Text.Length - 1)
                    {
                        txtTrLine.Text += "+";
                    }
                    else
                    {
                        txtTrLine.Text += "-";
                    }
                }
            }

            if (cbxTrObject.SelectedIndex == 1)
            {
                for (int i = 0; i < txtTrMat1.Text.Length; i++)
                {
                    if (i == 0 || i == txtTrMat1.Text.Length - 1)
                    {
                        txtTrLine.Text += "+";
                    }
                    else
                    {
                        txtTrLine.Text += "-";
                    }
                }
            }
        }   

        private void TrObjectSelection(object sender, SelectionChangedEventArgs e)
        {
            cbxTrObject.IsEnabled = false;


            if (cbxTrObject.SelectedIndex == 0)
            {
                txtTrObjectHeight.Text = "0";
                txtTrObjectHeight.IsEnabled = false;

                txtTrPointE.Visibility = Visibility.Hidden;
                txtTrPointF.Visibility = Visibility.Hidden;
                txtTrPointG.Visibility = Visibility.Hidden;
                txtTrPointH.Visibility = Visibility.Hidden;
            }

            if (cbxTrObject.SelectedIndex == 1)
            {
                txtTrObjectHeight.Text = "1";
                txtTrObjectHeight.IsEnabled = true;

                txtTrPointE.Visibility = Visibility.Visible;
                txtTrPointF.Visibility = Visibility.Visible;
                txtTrPointG.Visibility = Visibility.Visible;
                txtTrPointH.Visibility = Visibility.Visible;                
            }

            txtTrObjectLength.IsEnabled = true;
            txtTrObjectWidth.IsEnabled = true;            
            btnTrSetObject.IsEnabled = true;

            txtTrOriginX.IsEnabled = true;
            txtTrOriginY.IsEnabled = true;
            txtTrOriginZ.IsEnabled = true;
            btnTrSetOrigin.IsEnabled = true;

            txtTrMovementX.IsEnabled = true;
            txtTrMovementY.IsEnabled = true;
            txtTrMovementZ.IsEnabled = true;
            btnSetTranslation.IsEnabled = true;

            btnTranslate.IsEnabled = true;
        }

        private void Click_btnTrSetObject(object sender, RoutedEventArgs e)
        {
            txtTrObjectLength.IsEnabled = false;
            txtTrObjectWidth.IsEnabled = false;
            txtTrObjectHeight.IsEnabled = false;
            btnTrSetObject.IsEnabled = false;

            if (cbxTrObject.SelectedIndex == 0)
            {                
                txtTrObjectDescription.Text = String.Format("-> Persegi Panjang ABCD dengan panjang {0} dan lebar {1}",
                                                            txtTrObjectLength.Text, txtTrObjectWidth.Text);
            }

            if (cbxTrObject.SelectedIndex == 1)
            {    
                txtTrObjectDescription.Text = String.Format("-> Balok ABCD.EFGH dengan panjang {0}, lebar {1}, dan tinggi {2}",
                                                            txtTrObjectLength.Text, txtTrObjectWidth.Text, txtTrObjectHeight.Text);
            }
        }

        private void Click_btnTrSetOrigin(object sender, RoutedEventArgs e)
        {
            txtTrOriginX.IsEnabled = false;
            txtTrOriginY.IsEnabled = false;
            txtTrOriginZ.IsEnabled = false;
            btnTrSetOrigin.IsEnabled = false;

            TrX[0, 0] = Int32.Parse(txtTrOriginX.Text);
            TrX[1, 0] = Int32.Parse(txtTrOriginY.Text);
            TrX[2, 0] = Int32.Parse(txtTrOriginZ.Text);
            TrX[3, 0] = 1;

            TrX[0, 1] = Int32.Parse(txtTrOriginX.Text) + Int32.Parse(txtTrObjectLength.Text);
            TrX[1, 1] = Int32.Parse(txtTrOriginY.Text);
            TrX[2, 1] = Int32.Parse(txtTrOriginZ.Text);
            TrX[3, 1] = 1;

            TrX[0, 2] = Int32.Parse(txtTrOriginX.Text) + Int32.Parse(txtTrObjectLength.Text);
            TrX[1, 2] = Int32.Parse(txtTrOriginY.Text) + Int32.Parse(txtTrObjectWidth.Text);
            TrX[2, 2] = Int32.Parse(txtTrOriginZ.Text);
            TrX[3, 2] = 1;

            TrX[0, 3] = Int32.Parse(txtTrOriginX.Text);
            TrX[1, 3] = Int32.Parse(txtTrOriginY.Text) + Int32.Parse(txtTrObjectWidth.Text);
            TrX[2, 3] = Int32.Parse(txtTrOriginZ.Text);
            TrX[3, 3] = 1;

            TrX[0, 4] = Int32.Parse(txtTrOriginX.Text);
            TrX[1, 4] = Int32.Parse(txtTrOriginY.Text);
            TrX[2, 4] = Int32.Parse(txtTrOriginZ.Text) + Int32.Parse(txtTrObjectHeight.Text);
            TrX[3, 4] = 1;

            TrX[0, 5] = Int32.Parse(txtTrOriginX.Text) + Int32.Parse(txtTrObjectLength.Text);
            TrX[1, 5] = Int32.Parse(txtTrOriginY.Text);
            TrX[2, 5] = Int32.Parse(txtTrOriginZ.Text) + Int32.Parse(txtTrObjectHeight.Text);
            TrX[3, 5] = 1;

            TrX[0, 6] = Int32.Parse(txtTrOriginX.Text) + Int32.Parse(txtTrObjectLength.Text);
            TrX[1, 6] = Int32.Parse(txtTrOriginY.Text) + Int32.Parse(txtTrObjectWidth.Text);
            TrX[2, 6] = Int32.Parse(txtTrOriginZ.Text) + Int32.Parse(txtTrObjectHeight.Text);
            TrX[3, 6] = 1;

            TrX[0, 7] = Int32.Parse(txtTrOriginX.Text);
            TrX[1, 7] = Int32.Parse(txtTrOriginY.Text) + Int32.Parse(txtTrObjectWidth.Text);
            TrX[2, 7] = Int32.Parse(txtTrOriginZ.Text) + Int32.Parse(txtTrObjectHeight.Text);
            TrX[3, 7] = 1;

            txtTrPointA.Text = String.Format("   A ({0}, {1}, {2})",
                                             TrX[0, 0], TrX[1, 0], TrX[2, 0]);
            txtTrPointB.Text = String.Format("B ({0}, {1}, {2})",
                                             TrX[0, 1], TrX[1, 1], TrX[2, 1]);
            txtTrPointC.Text = String.Format("   C ({0}, {1}, {2})",
                                             TrX[0, 2], TrX[1, 2], TrX[2, 2]);
            txtTrPointD.Text = String.Format("D ({0}, {1}, {2})",
                                             TrX[0, 3], TrX[1, 3], TrX[2, 3]);
            txtTrPointE.Text = String.Format("   E ({0}, {1}, {2})",
                                             TrX[0, 4], TrX[1, 4], TrX[2, 4]);
            txtTrPointF.Text = String.Format("F ({0}, {1}, {2})",
                                             TrX[0, 5], TrX[1, 5], TrX[2, 5]);
            txtTrPointG.Text = String.Format("   G ({0}, {1}, {2})",
                                             TrX[0, 6], TrX[1, 6], TrX[2, 6]);
            txtTrPointH.Text = String.Format("H ({0}, {1}, {2})",
                                             TrX[0, 7], TrX[1, 7], TrX[2, 7]);

            TrDrawObjectOrigin();
        }

        private void Click_btnSetTranslation(object sender, RoutedEventArgs e)
        {
            txtTrMovementX.IsEnabled = false;
            txtTrMovementY.IsEnabled = false;
            txtTrMovementZ.IsEnabled = false;
            btnSetTranslation.IsEnabled = false;

            TrF[0, 0] = 1;
            TrF[1, 1] = 1;
            TrF[2, 2] = 1;
            TrF[3, 3] = 1;

            TrF[0, 3] = Int32.Parse(txtTrMovementX.Text);
            TrF[1, 3] = Int32.Parse(txtTrMovementY.Text);
            TrF[2, 3] = Int32.Parse(txtTrMovementZ.Text);

            txtTranslateDescription.Text = String.Format("   akan ditranslasi sejauh {0} pada sumbu x, {1} pada sumbu y, dan {2} pada sumbu z",
                                                         TrF[0, 3], TrF[1, 3], TrF[2, 3]);
        }

        private void Click_btnTranslate(object sender, RoutedEventArgs e)
        {
            btnTranslate.IsEnabled = false;
            btnTrReset.IsEnabled = true;

            MultiplyMatrix(TrY, TrF, TrX);

            if (cbxTrObject.SelectedIndex == 0)
            {
                txtTrMat0.Text = "-> [A' B' C' D'] = [T] * [A B C D]";
                txtTrMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                TrY[0, 0], TrY[0, 1], TrY[0, 2], TrY[0, 3],
                                                TrF[0, 0], TrF[0, 1], TrF[0, 2], TrF[0, 3],
                                                TrX[0, 0], TrX[0, 1], TrX[0, 2], TrX[0, 3]);
                txtTrMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} | = | {4,3} {5,3} {6,3} {7,3} | * | {8,3} {9,3} {10,3} {11,3} |",
                                                TrY[1, 0], TrY[1, 1], TrY[1, 2], TrY[1, 3],
                                                TrF[1, 0], TrF[1, 1], TrF[1, 2], TrF[1, 3],
                                                TrX[1, 0], TrX[1, 1], TrX[1, 2], TrX[1, 3]);
                txtTrMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                TrY[2, 0], TrY[2, 1], TrY[2, 2], TrY[2, 3],
                                                TrF[2, 0], TrF[2, 1], TrF[2, 2], TrF[2, 3],
                                                TrX[2, 0], TrX[2, 1], TrX[2, 2], TrX[2, 3]);
                txtTrMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                TrY[3, 0], TrY[3, 1], TrY[3, 2], TrY[3, 3],
                                                TrF[3, 0], TrF[3, 1], TrF[3, 2], TrF[3, 3],
                                                TrX[3, 0], TrX[3, 1], TrX[3, 2], TrX[3, 3]);
            }

            if (cbxTrObject.SelectedIndex == 1)
            {
                txtTrMat0.Text = "-> [A' B' C' D' E' F' G' H'] = [T] * [A B C D E F G H]";
                txtTrMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               TrY[0, 0], TrY[0, 1], TrY[0, 2], TrY[0, 3], TrY[0, 4], TrY[0, 5], TrY[0, 6], TrY[0, 7],
                                               TrF[0, 0], TrF[0, 1], TrF[0, 2], TrF[0, 3],
                                               TrX[0, 0], TrX[0, 1], TrX[0, 2], TrX[0, 3], TrX[0, 4], TrX[0, 5], TrX[0, 6], TrX[0, 7]);
                txtTrMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} | = | {8,3} {9,3} {10,3} {11,3} | * | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               TrY[1, 0], TrY[1, 1], TrY[1, 2], TrY[1, 3], TrY[1, 4], TrY[1, 5], TrY[1, 6], TrY[1, 7],
                                               TrF[1, 0], TrF[1, 1], TrF[1, 2], TrF[1, 3],
                                               TrX[1, 0], TrX[1, 1], TrX[1, 2], TrX[1, 3], TrX[1, 4], TrX[1, 5], TrX[1, 6], TrX[1, 7]);
                txtTrMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               TrY[2, 0], TrY[2, 1], TrY[2, 2], TrY[2, 3], TrY[2, 4], TrY[2, 5], TrY[2, 6], TrY[2, 7],
                                               TrF[2, 0], TrF[2, 1], TrF[2, 2], TrF[2, 3],
                                               TrX[2, 0], TrX[2, 1], TrX[2, 2], TrX[2, 3], TrX[2, 4], TrX[2, 5], TrX[2, 6], TrX[2, 7]);
                txtTrMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               TrY[3, 0], TrY[3, 1], TrY[3, 2], TrY[3, 3], TrY[3, 4], TrY[3, 5], TrY[3, 6], TrY[3, 7],
                                               TrF[3, 0], TrF[3, 1], TrF[3, 2], TrF[3, 3],
                                               TrX[3, 0], TrX[3, 1], TrX[3, 2], TrX[3, 3], TrX[3, 4], TrX[3, 5], TrX[3, 6], TrX[3, 7]);
            }            

            TrLineSeparator();
            
            TrDrawObjectDestination();
        }

        private void Click_btnTrReset(object sender, RoutedEventArgs e)
        {
            cbxTrObject.IsEnabled = true;            

            SetZeroMatrix(TrX);
            SetZeroMatrix(TrF);
            SetZeroMatrix(TrY);

            txtTrObjectLength.Text = "1";
            txtTrObjectWidth.Text = "1";
            txtTrObjectHeight.Text = "0";

            txtTrOriginX.Text = "0";
            txtTrOriginY.Text = "0";
            txtTrOriginZ.Text = "0";

            txtTrMovementX.Text = "0";
            txtTrMovementY.Text = "0";
            txtTrMovementZ.Text = "0";

            translationModelGroup.Children.Clear();
            TrDrawCartesianAxis();
        }

        //   _____  _____          _      _____ _   _  _____ 
        //  / ____|/ ____|   /\   | |    |_   _| \ | |/ ____|
        // | (___ | |       /  \  | |      | | |  \| | |  __ 
        //  \___ \| |      / /\ \ | |      | | | . ` | | |_ |
        //  ____) | |____ / ____ \| |____ _| |_| |\  | |__| |
        // |_____/ \_____/_/    \_\______|_____|_| \_|\_____|

        private void ScDrawCartesianAxis()
        {
            var axisBuilder = new MeshBuilder(false, false);            
            axisBuilder.AddPipe(new Point3D(-100, 0, 0), new Point3D(100, 0, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, -100, 0), new Point3D(0, 100, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, 0, -100), new Point3D(0, 0, 100), 0, 0.1, 360);

            var mesh = axisBuilder.ToMesh(true);
            var material = MaterialHelper.CreateMaterial(Colors.Gray);
            scalingModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = material
            });
            ScModelVisual.Content = scalingModelGroup;
        }

        public void ScDrawObjectOrigin()
        {
            var meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddBox(new Rect3D(ScX[0, 0], ScX[1, 0], ScX[2, 0], Int32.Parse(txtScObjectLength.Text), Int32.Parse(txtScObjectWidth.Text), Int32.Parse(txtScObjectHeight.Text)));

            var mesh = meshBuilder.ToMesh(true);

            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);

            scalingModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = blueMaterial
            });  
        }

        public void ScDrawObjectDestination()
        {
            var meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddBox(new Rect3D(ScX[0, 0], ScX[1, 0], ScX[2, 0], Int32.Parse(txtScObjectLength.Text) * ScF[0, 0], Int32.Parse(txtScObjectWidth.Text) * ScF[1, 1], Int32.Parse(txtScObjectHeight.Text) * ScF[2, 2]));

            var mesh = meshBuilder.ToMesh(true);

            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);

            scalingModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = redMaterial
            });
        }

        private void ScObjectList()
        {
            cbxScObject.Items.Add("Persegi Panjang");
            cbxScObject.Items.Add("Balok");
        }

        private void ScDisableAllInput()
        {
            txtScObjectLength.IsEnabled = false;
            txtScObjectWidth.IsEnabled = false;
            txtScObjectHeight.IsEnabled = false;
            btnScSetObject.IsEnabled = false;

            txtScOriginX.IsEnabled = false;
            txtScOriginY.IsEnabled = false;
            txtScOriginZ.IsEnabled = false;
            btnScSetOrigin.IsEnabled = false;

            txtScScaleX.IsEnabled = false;
            txtScScaleY.IsEnabled = false;
            txtScScaleZ.IsEnabled = false;
            btnSetScale.IsEnabled = false;

            btnScale.IsEnabled = false;
        }

        private void ScLineSeparator()
        {
            txtScLine.Text = String.Empty;

            if (cbxScObject.SelectedIndex == 0)
            {
                for (int i = 0; i < txtScaleDescription.Text.Length; i++)
                {
                    if (i == 0 || i == txtScaleDescription.Text.Length - 1)
                    {
                        txtTrLine.Text += "+";
                    }
                    else
                    {
                        txtTrLine.Text += "-";
                    }
                }
            }

            if (cbxScObject.SelectedIndex == 1)
            {
                for (int i = 0; i < txtScMat1.Text.Length; i++)
                {
                    if (i == 0 || i == txtScMat1.Text.Length - 1)
                    {
                        txtTrLine.Text += "+";
                    }
                    else
                    {
                        txtTrLine.Text += "-";
                    }
                }
            }
        }

        private void ScObjectSelection(object sender, SelectionChangedEventArgs e)
        {
            cbxScObject.IsEnabled = false;


            if (cbxScObject.SelectedIndex == 0)
            {
                txtScObjectHeight.Text = "0";
                txtScObjectHeight.IsEnabled = false;

                txtScPointE.Visibility = Visibility.Hidden;
                txtScPointF.Visibility = Visibility.Hidden;
                txtScPointG.Visibility = Visibility.Hidden;
                txtScPointH.Visibility = Visibility.Hidden;
            }

            if (cbxScObject.SelectedIndex == 1)
            {
                txtScObjectHeight.Text = "1";
                txtScObjectHeight.IsEnabled = true;

                txtScPointE.Visibility = Visibility.Visible;
                txtScPointF.Visibility = Visibility.Visible;
                txtScPointG.Visibility = Visibility.Visible;
                txtScPointH.Visibility = Visibility.Visible;
            }

            txtScObjectLength.IsEnabled = true;
            txtScObjectWidth.IsEnabled = true;
            btnScSetObject.IsEnabled = true;

            txtScOriginX.IsEnabled = true;
            txtScOriginY.IsEnabled = true;
            txtScOriginZ.IsEnabled = true;
            btnScSetOrigin.IsEnabled = true;

            txtScScaleX.IsEnabled = true;
            txtScScaleY.IsEnabled = true;
            txtScScaleZ.IsEnabled = true;
            btnSetScale.IsEnabled = true;

            btnScale.IsEnabled = true;
        }

        private void Click_btnScSetObject(object sender, RoutedEventArgs e)
        {
            txtScObjectLength.IsEnabled = false;
            txtScObjectWidth.IsEnabled = false;
            txtScObjectHeight.IsEnabled = false;
            btnScSetObject.IsEnabled = false;

            if (cbxScObject.SelectedIndex == 0)
            {
                txtScObjectDescription.Text = String.Format("-> Persegi Panjang ABCD dengan panjang {0} dan lebar {1}",
                                                            txtScObjectLength.Text, txtScObjectWidth.Text);
            }

            if (cbxScObject.SelectedIndex == 1)
            {
                txtScObjectDescription.Text = String.Format("-> Balok ABCD.EFGH dengan panjang {0}, lebar {1}, dan tinggi {2}",
                                                            txtScObjectLength.Text, txtScObjectWidth.Text, txtScObjectHeight.Text);
            }
        }

        private void Click_btnScSetOrigin(object sender, RoutedEventArgs e)
        {
            txtScOriginX.IsEnabled = false;
            txtScOriginY.IsEnabled = false;
            txtScOriginZ.IsEnabled = false;
            btnScSetOrigin.IsEnabled = false;

            ScX[0, 0] = Int32.Parse(txtScOriginX.Text);
            ScX[1, 0] = Int32.Parse(txtScOriginY.Text);
            ScX[2, 0] = Int32.Parse(txtScOriginZ.Text);
            ScX[3, 0] = 1;

            ScX[0, 1] = Int32.Parse(txtScOriginX.Text) + Int32.Parse(txtScObjectLength.Text);
            ScX[1, 1] = Int32.Parse(txtScOriginY.Text);
            ScX[2, 1] = Int32.Parse(txtScOriginZ.Text);
            ScX[3, 1] = 1;

            ScX[0, 2] = Int32.Parse(txtScOriginX.Text) + Int32.Parse(txtScObjectLength.Text);
            ScX[1, 2] = Int32.Parse(txtScOriginY.Text) + Int32.Parse(txtScObjectWidth.Text);
            ScX[2, 2] = Int32.Parse(txtScOriginZ.Text);
            ScX[3, 2] = 1;

            ScX[0, 3] = Int32.Parse(txtScOriginX.Text);
            ScX[1, 3] = Int32.Parse(txtScOriginY.Text) + Int32.Parse(txtScObjectWidth.Text);
            ScX[2, 3] = Int32.Parse(txtScOriginZ.Text);
            ScX[3, 3] = 1;
       
            ScX[0, 4] = Int32.Parse(txtScOriginX.Text);
            ScX[1, 4] = Int32.Parse(txtScOriginY.Text);
            ScX[2, 4] = Int32.Parse(txtScOriginZ.Text) + Int32.Parse(txtScObjectHeight.Text);
            ScX[3, 4] = 1;

            ScX[0, 5] = Int32.Parse(txtScOriginX.Text) + Int32.Parse(txtScObjectLength.Text);
            ScX[1, 5] = Int32.Parse(txtScOriginY.Text);
            ScX[2, 5] = Int32.Parse(txtScOriginZ.Text) + Int32.Parse(txtScObjectHeight.Text);
            ScX[3, 5] = 1;

            ScX[0, 6] = Int32.Parse(txtScOriginX.Text) + Int32.Parse(txtScObjectLength.Text);
            ScX[1, 6] = Int32.Parse(txtScOriginY.Text) + Int32.Parse(txtScObjectWidth.Text);
            ScX[2, 6] = Int32.Parse(txtScOriginZ.Text) + Int32.Parse(txtScObjectHeight.Text);
            ScX[3, 6] = 1;

            ScX[0, 7] = Int32.Parse(txtScOriginX.Text);
            ScX[1, 7] = Int32.Parse(txtScOriginY.Text) + Int32.Parse(txtScObjectWidth.Text);
            ScX[2, 7] = Int32.Parse(txtScOriginZ.Text) + Int32.Parse(txtScObjectHeight.Text);
            ScX[3, 7] = 1;

            txtScPointA.Text = String.Format("   A ({0}, {1}, {2})",
                                             ScX[0, 0], ScX[1, 0], ScX[2, 0]);
            txtScPointB.Text = String.Format("B ({0}, {1}, {2})",
                                             ScX[0, 1], ScX[1, 1], ScX[2, 1]);
            txtScPointC.Text = String.Format("   C ({0}, {1}, {2})",
                                             ScX[0, 2], ScX[1, 2], ScX[2, 2]);
            txtScPointD.Text = String.Format("D ({0}, {1}, {2})",
                                             ScX[0, 3], ScX[1, 3], ScX[2, 3]);
            txtScPointE.Text = String.Format("   E ({0}, {1}, {2})",
                                             ScX[0, 4], ScX[1, 4], ScX[2, 4]);
            txtScPointF.Text = String.Format("F ({0}, {1}, {2})",
                                             ScX[0, 5], ScX[1, 5], ScX[2, 5]);
            txtScPointG.Text = String.Format("   G ({0}, {1}, {2})",
                                             ScX[0, 6], ScX[1, 6], ScX[2, 6]);
            txtScPointH.Text = String.Format("H ({0}, {1}, {2})",
                                             ScX[0, 7], ScX[1, 7], ScX[2, 7]);

            ScDrawObjectOrigin();
        }

        private void Click_btnSetScale(object sender, RoutedEventArgs e)
        {
            txtScScaleX.IsEnabled = false;
            txtScScaleY.IsEnabled = false;
            txtScScaleZ.IsEnabled = false;
            btnSetScale.IsEnabled = false;

            ScF[0, 0] = Int32.Parse(txtScScaleX.Text);
            ScF[1, 1] = Int32.Parse(txtScScaleY.Text);
            ScF[2, 2] = Int32.Parse(txtScScaleZ.Text);
            ScF[3, 3] = 1;            

            txtScaleDescription.Text = String.Format("   akan diperbesar dengan perbesaran {0} pada sumbu x, {1} pada sumbu y, dan {2} pada sumbu z",
                                                         ScF[0, 0], ScF[1, 1], ScF[2, 2]);
        }

        private void Click_btnScale(object sender, RoutedEventArgs e)
        {
            btnScale.IsEnabled = false;
            btnScReset.IsEnabled = true;

            MultiplyMatrix(ScY, ScF, ScX);

            if (cbxScObject.SelectedIndex == 0)
            {
                txtScMat0.Text = "-> [A' B' C' D'] = [S] * [A B C D]";
                txtScMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                ScY[0, 0], ScY[0, 1], ScY[0, 2], ScY[0, 3],
                                                ScF[0, 0], ScF[0, 1], ScF[0, 2], ScF[0, 3],
                                                ScX[0, 0], ScX[0, 1], ScX[0, 2], ScX[0, 3]);
                txtScMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} | = | {4,3} {5,3} {6,3} {7,3} | * | {8,3} {9,3} {10,3} {11,3} |",
                                                ScY[1, 0], ScY[1, 1], ScY[1, 2], ScY[1, 3],
                                                ScF[1, 0], ScF[1, 1], ScF[1, 2], ScF[1, 3],
                                                ScX[1, 0], ScX[1, 1], ScX[1, 2], ScX[1, 3]);
                txtScMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                ScY[2, 0], ScY[2, 1], ScY[2, 2], ScY[2, 3],
                                                ScF[2, 0], ScF[2, 1], ScF[2, 2], ScF[2, 3],
                                                ScX[2, 0], ScX[2, 1], ScX[2, 2], ScX[2, 3]);
                txtScMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                ScY[3, 0], ScY[3, 1], ScY[3, 2], ScY[3, 3],
                                                ScF[3, 0], ScF[3, 1], ScF[3, 2], ScF[3, 3],
                                                ScX[3, 0], ScX[3, 1], ScX[3, 2], ScX[3, 3]);
            }

            if (cbxScObject.SelectedIndex == 1)
            {
                txtScMat0.Text = "-> [A' B' C' D' E' F' G' H'] = [S] * [A B C D E F G H]";
                txtScMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ScY[0, 0], ScY[0, 1], ScY[0, 2], ScY[0, 3], ScY[0, 4], ScY[0, 5], ScY[0, 6], ScY[0, 7],
                                               ScF[0, 0], ScF[0, 1], ScF[0, 2], ScF[0, 3],
                                               ScX[0, 0], ScX[0, 1], ScX[0, 2], ScX[0, 3], ScX[0, 4], ScX[0, 5], ScX[0, 6], ScX[0, 7]);
                txtScMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} | = | {8,3} {9,3} {10,3} {11,3} | * | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ScY[1, 0], ScY[1, 1], ScY[1, 2], ScY[1, 3], ScY[1, 4], ScY[1, 5], ScY[1, 6], ScY[1, 7],
                                               ScF[1, 0], ScF[1, 1], ScF[1, 2], ScF[1, 3],
                                               ScX[1, 0], ScX[1, 1], ScX[1, 2], ScX[1, 3], ScX[1, 4], ScX[1, 5], ScX[1, 6], ScX[1, 7]);
                txtScMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ScY[2, 0], ScY[2, 1], ScY[2, 2], ScY[2, 3], ScY[2, 4], ScY[2, 5], ScY[2, 6], ScY[2, 7],
                                               ScF[2, 0], ScF[2, 1], ScF[2, 2], ScF[2, 3],
                                               ScX[2, 0], ScX[2, 1], ScX[2, 2], ScX[2, 3], ScX[2, 4], ScX[2, 5], ScX[2, 6], ScX[2, 7]);
                txtScMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ScY[3, 0], ScY[3, 1], ScY[3, 2], ScY[3, 3], ScY[3, 4], ScY[3, 5], ScY[3, 6], ScY[3, 7],
                                               ScF[3, 0], ScF[3, 1], ScF[3, 2], ScF[3, 3],
                                               ScX[3, 0], ScX[3, 1], ScX[3, 2], ScX[3, 3], ScX[3, 4], ScX[3, 5], ScX[3, 6], ScX[3, 7]);
            }

            ScLineSeparator();

            ScDrawObjectDestination();
        }

        private void Click_btnScReset(object sender, RoutedEventArgs e)
        {
            cbxScObject.IsEnabled = true;

            SetZeroMatrix(ScX);
            SetZeroMatrix(ScF);
            SetZeroMatrix(ScY);

            txtScObjectLength.Text = "1";
            txtScObjectWidth.Text = "1";
            txtScObjectHeight.Text = "0";

            txtScOriginX.Text = "0";
            txtScOriginY.Text = "0";
            txtScOriginZ.Text = "0";

            txtScScaleX.Text = "1";
            txtScScaleY.Text = "1";
            txtScScaleZ.Text = "1";

            scalingModelGroup.Children.Clear();
            ScDrawCartesianAxis();            
        }
    }
}
