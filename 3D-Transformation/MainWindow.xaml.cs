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
using System.Windows.Media.Animation;
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
        Model3DGroup rotationModelGroup = new Model3DGroup();
        Model3DGroup shearingModelGroup = new Model3DGroup();

        private int[,] TrX = new int[4, 8];
        private int[,] TrF = new int[4, 4];
        private int[,] TrY = new int[4, 8];

        private int[,] ScX = new int[4, 8];
        private int[,] ScF = new int[4, 4];
        private int[,] ScY = new int[4, 8];

        private double[,] RoX = new double[4, 8];
        private double[,] RoF = new double[4, 12];
        private double[,] RoA = new double[4, 4];
        private double[,] RoB = new double[4, 4];
        private double[,] RoC = new double[4, 4];
        private double[,] RoD = new double[4, 8];
        private double[,] RoE = new double[4, 8];
        private double[,] RoM = new double[4, 4];
        private double[,] RoY = new double[4, 8];

        private int[,] ShX = new int[4, 8];
        private int[,] ShF = new int[4, 4];
        private int[,] ShY = new int[4, 8];

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

            SetZeroMatrix(RoX);
            SetZeroMatrix(RoA);
            SetZeroMatrix(RoB);
            SetZeroMatrix(RoC);
            SetZeroMatrix(RoD);
            SetZeroMatrix(RoE);
            SetZeroMatrix(RoM);
            SetZeroMatrix(RoY);
            RoObjectList();            
            RoDisableAllInput();
            RoDrawCartesianAxis();

            SetZeroMatrix(ShX);
            SetZeroMatrix(ShF);
            SetZeroMatrix(ShY);
            ShObjectList();
            ShDisableAllInput();
            ShDrawCartesianAxis();
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

        private void SetZeroMatrix(double[,] matrix)
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

        private void MultiplyMatrix(double[,] resultMatrix, double[,] firstMatrix, double[,] secondMatrix)
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

        // Translation
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

        // Scaling
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

            chkScHideObject.IsEnabled = false;

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

        private void Unchecked_chkScHideObject(object sender, RoutedEventArgs e)
        {
            ScDrawObjectDestination();
        }

        private void Checked_chkScHideObject(object sender, RoutedEventArgs e)
        {
            scalingModelGroup.Children.Clear();
            ScDrawCartesianAxis();
            ScDrawObjectOrigin();
        }

        private void Click_btnScale(object sender, RoutedEventArgs e)
        {
            btnScale.IsEnabled = false;
            btnScReset.IsEnabled = true;
            chkScHideObject.IsEnabled = true;

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

            chkScHideObject.IsEnabled = false;
            chkScHideObject.IsChecked = false;

            scalingModelGroup.Children.Clear();
            ScDrawCartesianAxis();            
        }

        // Rotation

        private void RoDrawCartesianAxis()
        {
            var axisBuilder = new MeshBuilder(false, false);
            axisBuilder.AddPipe(new Point3D(-100, 0, 0), new Point3D(100, 0, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, -100, 0), new Point3D(0, 100, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, 0, -100), new Point3D(0, 0, 100), 0, 0.1, 360);

            var mesh = axisBuilder.ToMesh(true);
            var material = MaterialHelper.CreateMaterial(Colors.Gray);
            rotationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = material
            });
            RoModelVisual.Content = rotationModelGroup;
        }

        public void RoDrawObjectOrigin()
        {
            var meshBuilder = new MeshBuilder(false, false);
            
            meshBuilder.AddPipe(new Point3D(RoX[0, 0], RoX[1, 0], RoX[2, 0]), new Point3D(RoX[0, 1], RoX[1, 1], RoX[2, 1]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(RoX[0, 0], RoX[1, 0], RoX[2, 0]), new Point3D(RoX[0, 3], RoX[1, 3], RoX[2, 3]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(RoX[0, 2], RoX[1, 2], RoX[2, 2]), new Point3D(RoX[0, 1], RoX[1, 1], RoX[2, 1]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(RoX[0, 2], RoX[1, 2], RoX[2, 2]), new Point3D(RoX[0, 3], RoX[1, 3], RoX[2, 3]), 0, 0.2, 360);

            /*meshBuilder.AddSphere(new Point3D(RoX[0, 0], RoX[1, 0], RoX[2, 0]), 0.1, 360, 360);
            meshBuilder.AddSphere(new Point3D(RoX[0, 1], RoX[1, 1], RoX[2, 1]), 0.1, 360, 360);
            meshBuilder.AddSphere(new Point3D(RoX[0, 2], RoX[1, 2], RoX[2, 2]), 0.1, 360, 360);            
            meshBuilder.AddSphere(new Point3D(RoX[0, 3], RoX[1, 3], RoX[2, 3]), 0.1, 360, 360);*/

            if (cbxRoObject.SelectedIndex == 1)
            {
                meshBuilder.AddPipe(new Point3D(RoX[0, 4], RoX[1, 4], RoX[2, 4]), new Point3D(RoX[0, 5], RoX[1, 5], RoX[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoX[0, 4], RoX[1, 4], RoX[2, 4]), new Point3D(RoX[0, 7], RoX[1, 7], RoX[2, 7]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoX[0, 6], RoX[1, 6], RoX[2, 6]), new Point3D(RoX[0, 5], RoX[1, 5], RoX[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoX[0, 6], RoX[1, 6], RoX[2, 6]), new Point3D(RoX[0, 7], RoX[1, 7], RoX[2, 7]), 0, 0.2, 360);
            
                meshBuilder.AddPipe(new Point3D(RoX[0, 0], RoX[1, 0], RoX[2, 0]), new Point3D(RoX[0, 4], RoX[1, 4], RoX[2, 4]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoX[0, 1], RoX[1, 1], RoX[2, 1]), new Point3D(RoX[0, 5], RoX[1, 5], RoX[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoX[0, 2], RoX[1, 2], RoX[2, 2]), new Point3D(RoX[0, 6], RoX[1, 6], RoX[2, 6]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoX[0, 3], RoX[1, 3], RoX[2, 3]), new Point3D(RoX[0, 7], RoX[1, 7], RoX[2, 7]), 0, 0.2, 360);

                /*meshBuilder.AddSphere(new Point3D(RoX[0, 4], RoX[1, 4], RoX[2, 4]), 0.1, 360, 360);
                meshBuilder.AddSphere(new Point3D(RoX[0, 5], RoX[1, 5], RoX[2, 5]), 0.1, 360, 360);
                meshBuilder.AddSphere(new Point3D(RoX[0, 6], RoX[1, 6], RoX[2, 6]), 0.1, 360, 360);
                meshBuilder.AddSphere(new Point3D(RoX[0, 7], RoX[1, 7], RoX[2, 7]), 0.1, 360, 360);*/
            }

            var mesh = meshBuilder.ToMesh(true);
            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);

            rotationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = blueMaterial
            });
        }

        private void RoDrawRotationAxis()
        {
            var axisBuilder = new MeshBuilder(false, false);
            if (cbxSumbuPutar.SelectedIndex == 0)
            {
                axisBuilder.AddPipe(new Point3D(-100, Int32.Parse(txtRoOriginY.Text), Int32.Parse(txtRoOriginZ.Text)), new Point3D(100, Int32.Parse(txtRoOriginY.Text), Int32.Parse(txtRoOriginZ.Text)), 0, 0.2, 360);
            }

            if (cbxSumbuPutar.SelectedIndex == 1)
            {
                axisBuilder.AddPipe(new Point3D(Int32.Parse(txtRoOriginX.Text), -100, Int32.Parse(txtRoOriginZ.Text)), new Point3D(Int32.Parse(txtRoOriginX.Text), 100, Int32.Parse(txtRoOriginZ.Text)), 0, 0.2, 360);
            }

            if (cbxSumbuPutar.SelectedIndex == 2)
            {
                axisBuilder.AddPipe(new Point3D(Int32.Parse(txtRoOriginX.Text), Int32.Parse(txtRoOriginY.Text), -100), new Point3D(Int32.Parse(txtRoOriginX.Text), Int32.Parse(txtRoOriginY.Text), 100), 0, 0.2, 360);
            }                        

            var mesh = axisBuilder.ToMesh(true);
            var material = MaterialHelper.CreateMaterial(Colors.Green);
            rotationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = material
            });
            RoModelVisual.Content = rotationModelGroup;
        }

        public void RoDrawObjectDestination()
        {
            var meshBuilder = new MeshBuilder(false, false);                        

            meshBuilder.AddPipe(new Point3D(RoY[0, 0], RoY[1, 0], RoY[2, 0]), new Point3D(RoY[0, 1], RoY[1, 1], RoY[2, 1]), 0, 0.2, 360);            
            meshBuilder.AddPipe(new Point3D(RoY[0, 0], RoY[1, 0], RoY[2, 0]), new Point3D(RoY[0, 3], RoY[1, 3], RoY[2, 3]), 0, 0.2, 360);            
            meshBuilder.AddPipe(new Point3D(RoY[0, 2], RoY[1, 2], RoY[2, 2]), new Point3D(RoY[0, 1], RoY[1, 1], RoY[2, 1]), 0, 0.2, 360);            
            meshBuilder.AddPipe(new Point3D(RoY[0, 2], RoY[1, 2], RoY[2, 2]), new Point3D(RoY[0, 3], RoY[1, 3], RoY[2, 3]), 0, 0.2, 360);            

            if (cbxRoObject.SelectedIndex == 1)
            {
                meshBuilder.AddPipe(new Point3D(RoY[0, 4], RoY[1, 4], RoY[2, 4]), new Point3D(RoY[0, 5], RoY[1, 5], RoY[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoY[0, 4], RoY[1, 4], RoY[2, 4]), new Point3D(RoY[0, 7], RoY[1, 7], RoY[2, 7]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoY[0, 6], RoY[1, 6], RoY[2, 6]), new Point3D(RoY[0, 5], RoY[1, 5], RoY[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoY[0, 6], RoY[1, 6], RoY[2, 6]), new Point3D(RoY[0, 7], RoY[1, 7], RoY[2, 7]), 0, 0.2, 360);

                meshBuilder.AddPipe(new Point3D(RoY[0, 0], RoY[1, 0], RoY[2, 0]), new Point3D(RoY[0, 4], RoY[1, 4], RoY[2, 4]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoY[0, 1], RoY[1, 1], RoY[2, 1]), new Point3D(RoY[0, 5], RoY[1, 5], RoY[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoY[0, 2], RoY[1, 2], RoY[2, 2]), new Point3D(RoY[0, 6], RoY[1, 6], RoY[2, 6]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(RoY[0, 3], RoY[1, 3], RoY[2, 3]), new Point3D(RoY[0, 7], RoY[1, 7], RoY[2, 7]), 0, 0.2, 360);
            }

            var mesh = meshBuilder.ToMesh(true);
            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);

            rotationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = redMaterial
            });            
        }

        private void RoObjectList()
        {
            cbxRoObject.Items.Add("Persegi Panjang");
            cbxRoObject.Items.Add("Balok");            
        }

        private void RoDisableAllInput()
        {
            txtRoObjectLength.IsEnabled = false;
            txtRoObjectWidth.IsEnabled = false;
            txtRoObjectHeight.IsEnabled = false;
            btnRoSetObject.IsEnabled = false;

            txtRoOriginX.IsEnabled = false;
            txtRoOriginY.IsEnabled = false;
            txtRoOriginZ.IsEnabled = false;
            btnRoSetOrigin.IsEnabled = false;

            cbxSumbuPutar.IsEnabled = false;
            txtSudutPutar.IsEnabled = false;
            btnRoSetRotation.IsEnabled = false;            

            btnRotate.IsEnabled = false;
        }

        private void RoLineSeparator()
        {
            txtRoLine.Text = String.Empty;

            if (cbxRoObject.SelectedIndex == 0)
            {
                for (int i = 0; i < txtRotateDescription.Text.Length; i++)
                {
                    if (i == 0 || i == txtRotateDescription.Text.Length - 1)
                    {
                        txtRoLine.Text += "+";
                    }
                    else
                    {
                        txtRoLine.Text += "-";
                    }
                }
            }

            if (cbxRoObject.SelectedIndex == 1)
            {
                for (int i = 0; i < txtRoMat1.Text.Length; i++)
                {
                    if (i == 0 || i == txtRoMat1.Text.Length - 1)
                    {
                        txtRoLine.Text += "+";
                    }
                    else
                    {
                        txtRoLine.Text += "-";
                    }
                }
            }
        }

        private void RoSumbuPutarList()
        {
            cbxSumbuPutar.Items.Clear();

            cbxSumbuPutar.Items.Add("Garis AB");
            cbxSumbuPutar.Items.Add("Garis AD");
            if (cbxRoObject.SelectedIndex == 1)
            {
                cbxSumbuPutar.Items.Add("Garis AE");
            }            
        }

        private void RoObjectSelection(object sender, SelectionChangedEventArgs e)
        {
            cbxRoObject.IsEnabled = false;

            if (cbxRoObject.SelectedIndex == 0)
            {
                txtRoObjectHeight.Text = "0";
                txtRoObjectHeight.IsEnabled = false;

                txtRoPointE.Visibility = Visibility.Hidden;
                txtRoPointF.Visibility = Visibility.Hidden;
                txtRoPointG.Visibility = Visibility.Hidden;
                txtRoPointH.Visibility = Visibility.Hidden;
            }

            if (cbxRoObject.SelectedIndex == 1)
            {
                txtRoObjectHeight.Text = "1";
                txtRoObjectHeight.IsEnabled = true;

                txtRoPointE.Visibility = Visibility.Visible;
                txtRoPointF.Visibility = Visibility.Visible;
                txtRoPointG.Visibility = Visibility.Visible;
                txtRoPointH.Visibility = Visibility.Visible;
            }

            RoSumbuPutarList();

            txtRoObjectLength.IsEnabled = true;
            txtRoObjectWidth.IsEnabled = true;
            btnRoSetObject.IsEnabled = true;

            txtRoOriginX.IsEnabled = true;
            txtRoOriginY.IsEnabled = true;
            txtRoOriginZ.IsEnabled = true;
            btnRoSetOrigin.IsEnabled = true;

            cbxSumbuPutar.IsEnabled = true;           

            btnRotate.IsEnabled = true;
        }

        private void SelectionChanged_cbxSumbuPutar(object sender, SelectionChangedEventArgs e)
        {
            txtSudutPutar.IsEnabled = true;
            btnRoSetRotation.IsEnabled = true;
        }

        private void Click_btnRoSetObject(object sender, RoutedEventArgs e)
        {
            txtRoObjectLength.IsEnabled =  false;
            txtRoObjectWidth.IsEnabled = false;
            txtRoObjectHeight.IsEnabled = false;
            btnRoSetObject.IsEnabled = false;

            if (cbxRoObject.SelectedIndex == 0)
            {
                txtRoObjectDescription.Text = String.Format("-> Persegi Panjang ABCD dengan panjang {0} dan lebar {1}",
                                                            txtRoObjectLength.Text, txtRoObjectWidth.Text);
            }

            if (cbxRoObject.SelectedIndex == 1)
            {
                txtRoObjectDescription.Text = String.Format("-> Balok ABCD.EFGH dengan panjang {0}, lebar {1}, dan tinggi {2}",
                                                            txtRoObjectLength.Text, txtRoObjectWidth.Text, txtRoObjectHeight.Text);
            }
        }

        private void Click_btnRoSetOrigin(object sender, RoutedEventArgs e)
        {
            txtRoOriginX.IsEnabled = false;
            txtRoOriginY.IsEnabled = false;
            txtRoOriginZ.IsEnabled = false;
            btnRoSetOrigin.IsEnabled = false;

            RoX[0, 0] = Int32.Parse(txtRoOriginX.Text);
            RoX[1, 0] = Int32.Parse(txtRoOriginY.Text);
            RoX[2, 0] = Int32.Parse(txtRoOriginZ.Text);
            RoX[3, 0] = 1;

            RoX[0, 1] = Int32.Parse(txtRoOriginX.Text) + Int32.Parse(txtRoObjectLength.Text);
            RoX[1, 1] = Int32.Parse(txtRoOriginY.Text);
            RoX[2, 1] = Int32.Parse(txtRoOriginZ.Text);
            RoX[3, 1] = 1;

            RoX[0, 2] = Int32.Parse(txtRoOriginX.Text) + Int32.Parse(txtRoObjectLength.Text);
            RoX[1, 2] = Int32.Parse(txtRoOriginY.Text) + Int32.Parse(txtRoObjectWidth.Text);
            RoX[2, 2] = Int32.Parse(txtRoOriginZ.Text);
            RoX[3, 2] = 1;

            RoX[0, 3] = Int32.Parse(txtRoOriginX.Text);
            RoX[1, 3] = Int32.Parse(txtRoOriginY.Text) + Int32.Parse(txtRoObjectWidth.Text);
            RoX[2, 3] = Int32.Parse(txtRoOriginZ.Text);
            RoX[3, 3] = 1;

            RoX[0, 4] = Int32.Parse(txtRoOriginX.Text);
            RoX[1, 4] = Int32.Parse(txtRoOriginY.Text);
            RoX[2, 4] = Int32.Parse(txtRoOriginZ.Text) + Int32.Parse(txtRoObjectHeight.Text);
            RoX[3, 4] = 1;

            RoX[0, 5] = Int32.Parse(txtRoOriginX.Text) + Int32.Parse(txtRoObjectLength.Text);
            RoX[1, 5] = Int32.Parse(txtRoOriginY.Text);
            RoX[2, 5] = Int32.Parse(txtRoOriginZ.Text) + Int32.Parse(txtRoObjectHeight.Text);
            RoX[3, 5] = 1;

            RoX[0, 6] = Int32.Parse(txtRoOriginX.Text) + Int32.Parse(txtRoObjectLength.Text);
            RoX[1, 6] = Int32.Parse(txtRoOriginY.Text) + Int32.Parse(txtRoObjectWidth.Text);
            RoX[2, 6] = Int32.Parse(txtRoOriginZ.Text) + Int32.Parse(txtRoObjectHeight.Text);
            RoX[3, 6] = 1;

            RoX[0, 7] = Int32.Parse(txtRoOriginX.Text);
            RoX[1, 7] = Int32.Parse(txtRoOriginY.Text) + Int32.Parse(txtRoObjectWidth.Text);
            RoX[2, 7] = Int32.Parse(txtRoOriginZ.Text) + Int32.Parse(txtRoObjectHeight.Text);
            RoX[3, 7] = 1;

            RoF[0, 8] = 1;
            RoF[0, 11] = -RoX[0, 0];

            RoF[1, 9] = 1;
            RoF[1, 11] = -RoX[1, 0];

            RoF[2, 10] = 1;
            RoF[2, 11] = -RoX[2, 0];

            RoF[3, 11] = 1;

            RoF[0, 0] = 1;
            RoF[0, 3] = RoX[0, 0];

            RoF[1, 1] = 1;
            RoF[1, 3] = RoX[1, 0];

            RoF[2, 2] = 1;
            RoF[2, 3] = RoX[2, 0];

            RoF[3, 3] = 1;

            txtRoPointA.Text = String.Format("   A ({0}, {1}, {2})",
                                             RoX[0, 0], RoX[1, 0], RoX[2, 0]);
            txtRoPointB.Text = String.Format("B ({0}, {1}, {2})",
                                             RoX[0, 1], RoX[1, 1], RoX[2, 1]);
            txtRoPointC.Text = String.Format("   C ({0}, {1}, {2})",
                                             RoX[0, 2], RoX[1, 2], RoX[2, 2]);
            txtRoPointD.Text = String.Format("D ({0}, {1}, {2})",
                                             RoX[0, 3], RoX[1, 3], RoX[2, 3]);
            txtRoPointE.Text = String.Format("   E ({0}, {1}, {2})",
                                             RoX[0, 4], RoX[1, 4], RoX[2, 4]);
            txtRoPointF.Text = String.Format("F ({0}, {1}, {2})",
                                             RoX[0, 5], RoX[1, 5], RoX[2, 5]);
            txtRoPointG.Text = String.Format("   G ({0}, {1}, {2})",
                                             RoX[0, 6], RoX[1, 6], RoX[2, 6]);
            txtRoPointH.Text = String.Format("H ({0}, {1}, {2})",
                                             RoX[0, 7], RoX[1, 7], RoX[2, 7]);

            RoDrawObjectOrigin();
        }

        private void Click_btnSetRotation(object sender, RoutedEventArgs e)
        {
            btnRoSetRotation.IsEnabled = false;

            double sudutPutar = Int32.Parse(txtSudutPutar.Text) * (Math.PI / 180);

            cbxSumbuPutar.IsEnabled = false;
            txtSudutPutar.IsEnabled = false;
            btnSetTranslation.IsEnabled = false;            

            if (cbxSumbuPutar.SelectedIndex == 0)
            {
                RoF[0, 4] = 1;
                RoF[1, 5] = Math.Cos(sudutPutar);
                RoF[1, 6] = -Math.Sin(sudutPutar);
                RoF[2, 5] = Math.Sin(sudutPutar);
                RoF[2, 6] = Math.Cos(sudutPutar);
                RoF[3, 7] = 1;
            }

            if (cbxSumbuPutar.SelectedIndex == 1)
            {                
                RoF[0, 4] = Math.Cos(sudutPutar);
                RoF[0, 6] = Math.Sin(sudutPutar);
                RoF[1, 5] = 1;
                RoF[2, 4] = -Math.Sin(sudutPutar);
                RoF[2, 6] = Math.Cos(sudutPutar);
                RoF[3, 7] = 1;
            }

            if (cbxSumbuPutar.SelectedIndex == 2)
            {                
                RoF[0, 4] = Math.Cos(sudutPutar);
                RoF[0, 5] = -Math.Sin(sudutPutar);
                RoF[1, 4] = Math.Sin(sudutPutar);
                RoF[1, 5] = Math.Cos(sudutPutar);
                RoF[2, 6] = 1;
                RoF[3, 7] = 1;
            }

            txtRotateDescription.Text = String.Format("   akan dirotasi {0} derajat dengan {1} sebagai sumbu putar",
                                                         txtSudutPutar.Text, cbxSumbuPutar.SelectedItem.ToString().ToLower());

            RoDrawRotationAxis();            
        }

        private void InitializeMatrix()
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    RoA[row, col] = RoF[row, col];
                    RoB[row, col] = RoF[row, col + 4];
                    RoC[row, col] = RoF[row, col + 8];
                }
            }
        }

        private void Click_btnRotate(object sender, RoutedEventArgs e)
        {
            btnRotate.IsEnabled = false;
            btnRoReset.IsEnabled = true;

            InitializeMatrix();            
            MultiplyMatrix(RoD, RoC, RoX);
            MultiplyMatrix(RoE, RoB, RoD);
            MultiplyMatrix(RoY, RoA, RoE);
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 4; j < 8; j++)
                {
                    RoF[i, j] = Math.Round(RoF[i, j], 2);
                }
            }

            txtRoMat0.Text = "-> [T'] * [R] * [T]";
            txtRoMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,5} {5,5} {6,5} {7,5} |   | {8,3} {9,3} {10,3} {11,3} |",
                                            RoF[0, 0], RoF[0, 1], RoF[0, 2], RoF[0, 3],
                                            RoF[0, 4], RoF[0, 5], RoF[0, 6], RoF[0, 7],
                                            RoF[0, 8], RoF[0, 9], RoF[0, 10], RoF[0, 11]);
            txtRoMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,5} {5,5} {6,5} {7,5} |   | {8,3} {9,3} {10,3} {11,3} |",
                                            RoF[1, 0], RoF[1, 1], RoF[1, 2], RoF[1, 3],
                                            RoF[1, 4], RoF[1, 5], RoF[1, 6], RoF[1, 7],
                                            RoF[1, 8], RoF[1, 9], RoF[1, 10], RoF[1, 11]);
            txtRoMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,5} {5,5} {6,5} {7,5} |   | {8,3} {9,3} {10,3} {11,3} |",                                               
                                            RoF[2, 0], RoF[2, 1], RoF[2, 2], RoF[2, 3],
                                            RoF[2, 4], RoF[2, 5], RoF[2, 6], RoF[2, 7],
                                            RoF[2, 8], RoF[2, 9], RoF[2, 10], RoF[2, 11]);
            txtRoMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,5} {5,5} {6,5} {7,5} |   | {8,3} {9,3} {10,3} {11,3} |",
                                            RoF[3, 0], RoF[3, 1], RoF[3, 2], RoF[3, 3],
                                            RoF[3, 4], RoF[3, 5], RoF[3, 6], RoF[3, 7],
                                            RoF[3, 8], RoF[3, 9], RoF[3, 10], RoF[3, 11]);                                                         

            RoLineSeparator();

            RoDrawObjectDestination();
        }

        private void Click_btnRoReset(object sender, RoutedEventArgs e)
        {
            cbxRoObject.IsEnabled = true;

            SetZeroMatrix(RoX);
            SetZeroMatrix(RoA);
            SetZeroMatrix(RoB);
            SetZeroMatrix(RoC);
            SetZeroMatrix(RoD);
            SetZeroMatrix(RoE);
            SetZeroMatrix(RoF);
            SetZeroMatrix(RoM);
            SetZeroMatrix(RoY);

            txtRoObjectLength.Text = "1";
            txtRoObjectWidth.Text = "1";
            txtRoObjectHeight.Text = "0";

            txtRoOriginX.Text = "0";
            txtRoOriginY.Text = "0";
            txtRoOriginZ.Text = "0";

            txtSudutPutar.Text = "0";

            rotationModelGroup.Children.Clear();
            RoDrawCartesianAxis();
        }

        //Shearing

        private void ShDrawCartesianAxis()
        {
            var axisBuilder = new MeshBuilder(true, true, true);
            axisBuilder.AddPipe(new Point3D(-100, 0, 0), new Point3D(100, 0, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, -100, 0), new Point3D(0, 100, 0), 0, 0.1, 360);
            axisBuilder.AddPipe(new Point3D(0, 0, -100), new Point3D(0, 0, 100), 0, 0.1, 360);

            var mesh = axisBuilder.ToMesh(true);
            var material = MaterialHelper.CreateMaterial(Colors.Gray);
            shearingModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = material
            });
            ShModelVisual.Content = translationModelGroup;
        }

        public void ShDrawObjectOrigin()
        {
            var meshBuilder = new MeshBuilder(false, false);

            meshBuilder.AddPipe(new Point3D(ShX[0, 0], ShX[1, 0], ShX[2, 0]), new Point3D(ShX[0, 1], ShX[1, 1], ShX[2, 1]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(ShX[0, 0], ShX[1, 0], ShX[2, 0]), new Point3D(ShX[0, 3], ShX[1, 3], ShX[2, 3]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(ShX[0, 2], ShX[1, 2], ShX[2, 2]), new Point3D(ShX[0, 1], ShX[1, 1], ShX[2, 1]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(ShX[0, 2], ShX[1, 2], ShX[2, 2]), new Point3D(ShX[0, 3], ShX[1, 3], ShX[2, 3]), 0, 0.2, 360);

            if (cbxShObject.SelectedIndex == 1)
            {
                meshBuilder.AddPipe(new Point3D(ShX[0, 4], ShX[1, 4], ShX[2, 4]), new Point3D(ShX[0, 5], ShX[1, 5], ShX[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShX[0, 4], ShX[1, 4], ShX[2, 4]), new Point3D(ShX[0, 7], ShX[1, 7], ShX[2, 7]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShX[0, 6], ShX[1, 6], ShX[2, 6]), new Point3D(ShX[0, 5], ShX[1, 5], ShX[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShX[0, 6], ShX[1, 6], ShX[2, 6]), new Point3D(ShX[0, 7], ShX[1, 7], ShX[2, 7]), 0, 0.2, 360);

                meshBuilder.AddPipe(new Point3D(ShX[0, 0], ShX[1, 0], ShX[2, 0]), new Point3D(ShX[0, 4], ShX[1, 4], ShX[2, 4]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShX[0, 1], ShX[1, 1], ShX[2, 1]), new Point3D(ShX[0, 5], ShX[1, 5], ShX[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShX[0, 2], ShX[1, 2], ShX[2, 2]), new Point3D(ShX[0, 6], ShX[1, 6], ShX[2, 6]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShX[0, 3], ShX[1, 3], ShX[2, 3]), new Point3D(ShX[0, 7], ShX[1, 7], ShX[2, 7]), 0, 0.2, 360);
            }

            var mesh = meshBuilder.ToMesh(true);

            var blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);

            translationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = blueMaterial
            });
        }

        public void ShDrawObjectDestination()
        {
            var meshBuilder = new MeshBuilder(false, false);

            meshBuilder.AddPipe(new Point3D(ShY[0, 0], ShY[1, 0], ShY[2, 0]), new Point3D(ShY[0, 1], ShY[1, 1], ShY[2, 1]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(ShY[0, 0], ShY[1, 0], ShY[2, 0]), new Point3D(ShY[0, 3], ShY[1, 3], ShY[2, 3]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(ShY[0, 2], ShY[1, 2], ShY[2, 2]), new Point3D(ShY[0, 1], ShY[1, 1], ShY[2, 1]), 0, 0.2, 360);
            meshBuilder.AddPipe(new Point3D(ShY[0, 2], ShY[1, 2], ShY[2, 2]), new Point3D(ShY[0, 3], ShY[1, 3], ShY[2, 3]), 0, 0.2, 360);

            if (cbxShObject.SelectedIndex == 1)
            {
                meshBuilder.AddPipe(new Point3D(ShY[0, 4], ShY[1, 4], ShY[2, 4]), new Point3D(ShY[0, 5], ShY[1, 5], ShY[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShY[0, 4], ShY[1, 4], ShY[2, 4]), new Point3D(ShY[0, 7], ShY[1, 7], ShY[2, 7]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShY[0, 6], ShY[1, 6], ShY[2, 6]), new Point3D(ShY[0, 5], ShY[1, 5], ShY[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShY[0, 6], ShY[1, 6], ShY[2, 6]), new Point3D(ShY[0, 7], ShY[1, 7], ShY[2, 7]), 0, 0.2, 360);

                meshBuilder.AddPipe(new Point3D(ShY[0, 0], ShY[1, 0], ShY[2, 0]), new Point3D(ShY[0, 4], ShY[1, 4], ShY[2, 4]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShY[0, 1], ShY[1, 1], ShY[2, 1]), new Point3D(ShY[0, 5], ShY[1, 5], ShY[2, 5]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShY[0, 2], ShY[1, 2], ShY[2, 2]), new Point3D(ShY[0, 6], ShY[1, 6], ShY[2, 6]), 0, 0.2, 360);
                meshBuilder.AddPipe(new Point3D(ShY[0, 3], ShY[1, 3], ShY[2, 3]), new Point3D(ShY[0, 7], ShY[1, 7], ShY[2, 7]), 0, 0.2, 360);
            }

            var mesh = meshBuilder.ToMesh(true);

            var redMaterial = MaterialHelper.CreateMaterial(Colors.Red);

            translationModelGroup.Children.Add(new GeometryModel3D
            {
                Geometry = mesh,
                Material = redMaterial
            });
        }

        private void ShObjectList()
        {
            cbxShObject.Items.Add("Persegi Panjang");
            cbxShObject.Items.Add("Balok");
        }

        private void ShDisableAllInput()
        {
            txtShObjectLength.IsEnabled = false;
            txtShObjectWidth.IsEnabled = false;
            txtShObjectHeight.IsEnabled = false;
            btnShSetObject.IsEnabled = false;

            txtShOriginX.IsEnabled = false;
            txtShOriginY.IsEnabled = false;
            txtShOriginZ.IsEnabled = false;
            btnShSetOrigin.IsEnabled = false;

            txtShXY.IsEnabled = false;
            txtShXZ.IsEnabled = false;
            txtShYX.IsEnabled = false;
            txtShYZ.IsEnabled = false;
            txtShZX.IsEnabled = false;
            txtShZY.IsEnabled = false;
            btnSetShear.IsEnabled = false;

            btnShear.IsEnabled = false;
        }

        private void ShLineSeparator()
        {
            txtShLine.Text = String.Empty;

            if (cbxShObject.SelectedIndex == 0)
            {
                for (int i = 0; i < txtShearDescription.Text.Length; i++)
                {
                    if (i == 0 || i == txtShearDescription.Text.Length - 1)
                    {
                        txtShLine.Text += "+";
                    }
                    else
                    {
                        txtShLine.Text += "-";
                    }
                }
            }

            if (cbxShObject.SelectedIndex == 1)
            {
                for (int i = 0; i < txtShMat1.Text.Length; i++)
                {
                    if (i == 0 || i == txtShMat1.Text.Length - 1)
                    {
                        txtShLine.Text += "+";
                    }
                    else
                    {
                        txtShLine.Text += "-";
                    }
                }
            }
        }

        private void ShObjectSelection(object sender, SelectionChangedEventArgs e)
        {
            cbxShObject.IsEnabled = false;


            if (cbxShObject.SelectedIndex == 0)
            {
                txtShObjectHeight.Text = "0";
                txtShObjectHeight.IsEnabled = false;

                txtShPointE.Visibility = Visibility.Hidden;
                txtShPointF.Visibility = Visibility.Hidden;
                txtShPointG.Visibility = Visibility.Hidden;
                txtShPointH.Visibility = Visibility.Hidden;
            }

            if (cbxShObject.SelectedIndex == 1)
            {
                txtShObjectHeight.Text = "1";
                txtShObjectHeight.IsEnabled = true;

                txtShPointE.Visibility = Visibility.Visible;
                txtShPointF.Visibility = Visibility.Visible;
                txtShPointG.Visibility = Visibility.Visible;
                txtShPointH.Visibility = Visibility.Visible;
            }

            txtShObjectLength.IsEnabled = true;
            txtShObjectWidth.IsEnabled = true;
            btnShSetObject.IsEnabled = true;

            txtShOriginX.IsEnabled = true;
            txtShOriginY.IsEnabled = true;
            txtShOriginZ.IsEnabled = true;
            btnShSetOrigin.IsEnabled = true;

            txtShXY.IsEnabled = true;
            txtShXZ.IsEnabled = true;
            txtShYX.IsEnabled = true;
            txtShYZ.IsEnabled = true;
            txtShZX.IsEnabled = true;
            txtShZY.IsEnabled = true;
            btnSetShear.IsEnabled = true;

            btnShear.IsEnabled = true;
        }

        private void Click_btnShSetObject(object sender, RoutedEventArgs e)
        {
            txtShObjectLength.IsEnabled = false;
            txtShObjectWidth.IsEnabled = false;
            txtShObjectHeight.IsEnabled = false;
            btnShSetObject.IsEnabled = false;

            if (cbxShObject.SelectedIndex == 0)
            {
                txtShObjectDescription.Text = String.Format("-> Persegi Panjang ABCD dengan panjang {0} dan lebar {1}",
                                                            txtShObjectLength.Text, txtTrObjectWidth.Text);
            }

            if (cbxShObject.SelectedIndex == 1)
            {
                txtShObjectDescription.Text = String.Format("-> Balok ABCD.EFGH dengan panjang {0}, lebar {1}, dan tinggi {2}",
                                                            txtShObjectLength.Text, txtTrObjectWidth.Text, txtTrObjectHeight.Text);
            }
        }

        private void Click_btnShSetOrigin(object sender, RoutedEventArgs e)
        {
            txtShOriginX.IsEnabled = false;
            txtShOriginY.IsEnabled = false;
            txtShOriginZ.IsEnabled = false;
            btnShSetOrigin.IsEnabled = false;

            ShX[0, 0] = Int32.Parse(txtShOriginX.Text);
            ShX[1, 0] = Int32.Parse(txtShOriginY.Text);
            ShX[2, 0] = Int32.Parse(txtShOriginZ.Text);
            ShX[3, 0] = 1;

            ShX[0, 1] = Int32.Parse(txtShOriginX.Text) + Int32.Parse(txtShObjectLength.Text);
            ShX[1, 1] = Int32.Parse(txtShOriginY.Text);
            ShX[2, 1] = Int32.Parse(txtShOriginZ.Text);
            ShX[3, 1] = 1;

            ShX[0, 2] = Int32.Parse(txtShOriginX.Text) + Int32.Parse(txtShObjectLength.Text);
            ShX[1, 2] = Int32.Parse(txtShOriginY.Text) + Int32.Parse(txtShObjectWidth.Text);
            ShX[2, 2] = Int32.Parse(txtShOriginZ.Text);
            ShX[3, 2] = 1;

            ShX[0, 3] = Int32.Parse(txtShOriginX.Text);
            ShX[1, 3] = Int32.Parse(txtShOriginY.Text) + Int32.Parse(txtShObjectWidth.Text);
            ShX[2, 3] = Int32.Parse(txtShOriginZ.Text);
            ShX[3, 3] = 1;

            ShX[0, 4] = Int32.Parse(txtShOriginX.Text);
            ShX[1, 4] = Int32.Parse(txtShOriginY.Text);
            ShX[2, 4] = Int32.Parse(txtShOriginZ.Text) + Int32.Parse(txtShObjectHeight.Text);
            ShX[3, 4] = 1;

            ShX[0, 5] = Int32.Parse(txtShOriginX.Text) + Int32.Parse(txtShObjectLength.Text);
            ShX[1, 5] = Int32.Parse(txtShOriginY.Text);
            ShX[2, 5] = Int32.Parse(txtShOriginZ.Text) + Int32.Parse(txtShObjectHeight.Text);
            ShX[3, 5] = 1;

            ShX[0, 6] = Int32.Parse(txtShOriginX.Text) + Int32.Parse(txtShObjectLength.Text);
            ShX[1, 6] = Int32.Parse(txtShOriginY.Text) + Int32.Parse(txtShObjectWidth.Text);
            ShX[2, 6] = Int32.Parse(txtShOriginZ.Text) + Int32.Parse(txtShObjectHeight.Text);
            ShX[3, 6] = 1;

            ShX[0, 7] = Int32.Parse(txtShOriginX.Text);
            ShX[1, 7] = Int32.Parse(txtShOriginY.Text) + Int32.Parse(txtShObjectWidth.Text);
            ShX[2, 7] = Int32.Parse(txtShOriginZ.Text) + Int32.Parse(txtShObjectHeight.Text);
            ShX[3, 7] = 1;

            txtShPointA.Text = String.Format("   A ({0}, {1}, {2})",
                                             ShX[0, 0], ShX[1, 0], ShX[2, 0]);
            txtShPointB.Text = String.Format("B ({0}, {1}, {2})",
                                             ShX[0, 1], ShX[1, 1], ShX[2, 1]);
            txtShPointC.Text = String.Format("   C ({0}, {1}, {2})",
                                             ShX[0, 2], ShX[1, 2], ShX[2, 2]);
            txtShPointD.Text = String.Format("D ({0}, {1}, {2})",
                                            ShX[0, 3], ShX[1, 3], ShX[2, 3]);
            txtShPointE.Text = String.Format("   E ({0}, {1}, {2})",
                                             ShX[0, 4], ShX[1, 4], ShX[2, 4]);
            txtShPointF.Text = String.Format("F ({0}, {1}, {2})",
                                             ShX[0, 5], ShX[1, 5], ShX[2, 5]);
            txtShPointG.Text = String.Format("   G ({0}, {1}, {2})",
                                             ShX[0, 6], ShX[1, 6], ShX[2, 6]);
            txtShPointH.Text = String.Format("H ({0}, {1}, {2})",
                                             ShX[0, 7], ShX[1, 7], ShX[2, 7]);

            ShDrawObjectOrigin();
        }

        private void Click_btnSetShear(object sender, RoutedEventArgs e)
        {
            txtShXY.IsEnabled = false;
            txtShXZ.IsEnabled = false;
            txtShYX.IsEnabled = false;
            txtShYZ.IsEnabled = false;
            txtShZX.IsEnabled = false;
            txtShZY.IsEnabled = false;
            btnSetShear.IsEnabled = false;

            ShF[0, 0] = 1;
            ShF[0, 1] = Int32.Parse(txtShXY.Text);
            ShF[0, 2] = Int32.Parse(txtShXZ.Text);

            ShF[1, 1] = 1;
            ShF[1, 0] = Int32.Parse(txtShYX.Text);
            ShF[1, 2] = Int32.Parse(txtShYZ.Text);

            ShF[2, 2] = 1;
            ShF[2, 0] = Int32.Parse(txtShZX.Text);
            ShF[2, 1] = Int32.Parse(txtShZY.Text);

            ShF[3, 3] = 1;

            //txtTranslateDescription.Text = String.Format("   akan ditranslasi sejauh {0} pada sumbu x, {1} pada sumbu y, dan {2} pada sumbu z",
            //                                             TrF[0, 3], TrF[1, 3], TrF[2, 3]);
        }

        private void Click_btnShear(object sender, RoutedEventArgs e)
        {
            btnShear.IsEnabled = false;
            btnShReset.IsEnabled = true;

            MultiplyMatrix(ShY, ShF, ShX);

            if (cbxShObject.SelectedIndex == 0)
            {
                txtShMat0.Text = "-> [A' B' C' D'] = [S] * [A B C D]";
                txtShMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                ShY[0, 0], ShY[0, 1], ShY[0, 2], ShY[0, 3],
                                                ShF[0, 0], ShF[0, 1], ShF[0, 2], ShF[0, 3],
                                                ShX[0, 0], ShX[0, 1], ShX[0, 2], ShX[0, 3]);
                txtShMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} | = | {4,3} {5,3} {6,3} {7,3} | * | {8,3} {9,3} {10,3} {11,3} |",
                                                ShY[1, 0], ShY[1, 1], ShY[1, 2], ShY[1, 3],
                                                ShF[1, 0], ShF[1, 1], ShF[1, 2], ShF[1, 3],
                                                ShX[1, 0], ShX[1, 1], ShX[1, 2], ShX[1, 3]);
                txtShMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                ShY[2, 0], ShY[2, 1], ShY[2, 2], ShY[2, 3],
                                                ShF[2, 0], ShF[2, 1], ShF[2, 2], ShF[2, 3],
                                                ShX[2, 0], ShX[2, 1], ShX[2, 2], ShX[2, 3]);
                txtShMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} |   | {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |",
                                                ShY[3, 0], ShY[3, 1], ShY[3, 2], ShY[3, 3],
                                                ShF[3, 0], ShF[3, 1], ShF[3, 2], ShF[3, 3],
                                                ShX[3, 0], ShX[3, 1], ShX[3, 2], ShX[3, 3]);
            }

            if (cbxShObject.SelectedIndex == 1)
            {
                txtShMat0.Text = "-> [A' B' C' D' E' F' G' H'] = [T] * [A B C D E F G H]";
                txtShMat1.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ShY[0, 0], ShY[0, 1], ShY[0, 2], ShY[0, 3], ShY[0, 4], ShY[0, 5], ShY[0, 6], ShY[0, 7],
                                               ShF[0, 0], ShF[0, 1], ShF[0, 2], ShF[0, 3],
                                               ShX[0, 0], ShX[0, 1], ShX[0, 2], ShX[0, 3], ShX[0, 4], ShX[0, 5], ShX[0, 6], ShX[0, 7]);
                txtShMat2.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} | = | {8,3} {9,3} {10,3} {11,3} | * | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ShY[1, 0], ShY[1, 1], ShY[1, 2], ShY[1, 3], ShY[1, 4], ShY[1, 5], ShY[1, 6], ShY[1, 7],
                                               ShF[1, 0], ShF[1, 1], ShF[1, 2], ShF[1, 3],
                                               ShX[1, 0], ShX[1, 1], ShX[1, 2], ShX[1, 3], ShX[1, 4], ShX[1, 5], ShX[1, 6], ShX[1, 7]);
                txtShMat3.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ShY[2, 0], ShY[2, 1], ShY[2, 2], ShY[2, 3], ShY[2, 4], ShY[2, 5], ShY[2, 6], ShY[2, 7],
                                               ShF[2, 0], ShF[2, 1], ShF[2, 2], ShF[2, 3],
                                               ShX[2, 0], ShX[2, 1], ShX[2, 2], ShX[2, 3], ShX[2, 4], ShX[2, 5], ShX[2, 6], ShX[2, 7]);
                txtShMat4.Text = String.Format("   | {0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} |   | {8,3} {9,3} {10,3} {11,3} |   | {12,3} {13,3} {14,3} {15,3} {16,3} {17,3} {18,3} {19,3} |",
                                               ShY[3, 0], ShY[3, 1], ShY[3, 2], ShY[3, 3], ShY[3, 4], ShY[3, 5], ShY[3, 6], ShY[3, 7],
                                               ShF[3, 0], ShF[3, 1], ShF[3, 2], ShF[3, 3],
                                               ShX[3, 0], ShX[3, 1], ShX[3, 2], ShX[3, 3], ShX[3, 4], ShX[3, 5], ShX[3, 6], ShX[3, 7]);
            }

            ShLineSeparator();

            ShDrawObjectDestination();
        }

        private void Click_btnShReset(object sender, RoutedEventArgs e)
        {
            cbxShObject.IsEnabled = true;

            SetZeroMatrix(ShX);
            SetZeroMatrix(ShF);
            SetZeroMatrix(ShY);

            txtShObjectLength.Text = "1";
            txtShObjectWidth.Text = "1";
            txtShObjectHeight.Text = "0";

            txtShOriginX.Text = "0";
            txtShOriginY.Text = "0";
            txtShOriginZ.Text = "0";

            txtShXY.Text = "0";
            txtShXY.Text = "0";
            txtShXY.Text = "0";
            txtShXY.Text = "0";
            txtShXY.Text = "0";
            txtShXY.Text = "0";            

            shearingModelGroup.Children.Clear();
            ShDrawCartesianAxis();
        }

    }
}