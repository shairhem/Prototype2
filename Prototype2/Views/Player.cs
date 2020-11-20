using System.Windows.Forms;

namespace Prototype2.Views
{
    public partial class Player : Form
    {
        private readonly string filePath;

        public Player(string filePath)
        {
            this.filePath = filePath;
            InitializeComponent();
        }
    }
}
