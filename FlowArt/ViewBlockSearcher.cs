using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using System.Windows.Forms;
using System.Drawing;


namespace FlowArt
{
    class ViewBlockSearcher
    {
        private GoCollectionEnumerator SearchEnumerator;   
        public EventHandler eventSearchReset;
        public bool SearchCaseSensitive = false;
        private FlowView ViewToSearch;

        public ViewBlockSearcher(FlowView view)
        {
            this.ViewToSearch = view;
        }

        private bool searchReset = true;
        public bool SearchReset
        {
            get
            {
                return this.searchReset;
            }
            set
            {
                if(value)
                    eventSearchReset?.Invoke(this, new EventArgs());           
                this.searchReset = value;
            }
        }

        private void SearchResetEnumerator()
        {
            // Automatically choose all object if there is less than 2 selected objects
            if(ViewToSearch.Selection==null || ViewToSearch.Selection.Count <= 1)
                ViewToSearch.SelectAll();

            // if there is still no object selected, return, otherwise start iterating
            if(ViewToSearch.Selection!=null && ViewToSearch.Selection.Count > 0)
            {
                SearchEnumerator = ViewToSearch.Selection.GetEnumerator();
                SearchReset = false;
            }
            else
                return;
        }

        private bool MatchedBlock(FlowBlock Block, string KeyWord)
        {
            string Text = Block.Text;
            if( ! SearchCaseSensitive )
            {
                Text = Text.ToUpper();
                KeyWord = KeyWord.ToUpper();
            }
            int index = Text.IndexOf(KeyWord);
            return (index >= 0);
        }

        public FlowBlock SearchNextBlock(string KeyWord)
        {
            if( SearchReset )
            {
                SearchResetEnumerator();
            }

            // if search is still not available at the yet
            if( SearchReset )
            {
                // invoke the event again
                SearchReset = true;
                return null;
            }

            // start iterating the collection
            while( true )
            {
                try
                {
                    if(! SearchEnumerator.MoveNext() )
                    {
                        MessageBox.Show("No match left!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SearchReset = true;
                        return null;
                    }
                }
                catch
                {
                    SearchReset = true;
                    return null;
                }

                if(SearchEnumerator.Current is FlowBlock)
                {
                    FlowBlock Block = SearchEnumerator.Current as FlowBlock;
                    if ( MatchedBlock(Block, KeyWord) )
                        return Block;
                }
            }
        }

    

    }
}
