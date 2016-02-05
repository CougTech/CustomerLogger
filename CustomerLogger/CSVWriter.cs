using System;
using System.Collections.Generic;
using System.Linq;
//Ryan Huard
//v 1.0.0

//Usage
//CSVWriter w = new CSVWriter(file_name.csv)
//Add to line:
//      w.addToCurrent("some string") will add "some string" at the end of the line as a new cell
//      w.addToStart("some string") will add "some string" at the front of the line in a new cell
//      after you have completed the line:
//w.WriteLine


using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSV {
    public class CSVWriter {
        private string _line;
        private FileStream _fs;
        private StreamWriter _sw;
        private string _fname;

        public CSVWriter(string fname) {

            _line = "";
            _fname = fname;

            if(true == File.Exists(_fname)) { 
            
                File.Delete(_fname);
            }

        }

        public void addToCurrent(string text) {

            if(_line == "") {
                _line = text;
            } else {
                _line = _line + "," + text;
            }
        }

        public void addToStart(string text) {

            if(_line == "") { 
            
                _line = text;
            } else { 
            
                _line = text + "," + _line;
            }
        }

        public void WriteLine() {

            using(_fs = new FileStream(_fname, FileMode.Append))
            using(_sw = new StreamWriter(_fs)) {
                _sw.WriteLine(_line);
            }
            _line = "";
        }

    }
}
