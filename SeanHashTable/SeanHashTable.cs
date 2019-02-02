using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeanHashTable
{
    class SeanHashTable
    {
        private string[] table = new string[1000];
        private string[] resizingTable;
        private bool resizing = false;
        private int resizeIndex = 0;
        public int Count { get; private set; }
        public SeanHashTable() { Count = 0; }

        /// <summary>
        /// Adds a string to the hash table.
        /// </summary>
        /// <param name="newValue">The string to be added to the table.</param>
        public void Add(string newValue)
        {
            if (resizing || Count >= table.Length * .75)
            {
                WorkOnResize();
            }

            PlaceInTable(newValue);
            Count++;
        }

        private void WorkOnResize()
        {
            bool finished = false;
            int cycles = 1;

            if (!resizing)
            {
                resizing = true;
                resizingTable = new string[table.Length * 2];
            }

            while (!finished && cycles < 4)
            {
                if (table[resizeIndex] != null)
                {
                    PlaceInTable(table[resizeIndex]);
                }
                resizeIndex++;
                if (resizeIndex >= table.Length)
                {
                    resizing = false;
                    resizeIndex = 0;
                    table = resizingTable;
                    resizingTable = new string[0];
                    resizingTable = null;
                    finished = true;
                }

                cycles++;
            }
        }

        private void PlaceInTable(string value)
        {
            int index = GetHashIndex(value);
            string[] tableToEdit;

            if (resizing)
            {
                tableToEdit = resizingTable;
            }
            else
            {
                tableToEdit = table;
            }

            if (tableToEdit[index] == null)
            {
                tableToEdit[index] = value;
            }
            else
            {
                index = FindEmptyIndex(index);
                bool placed = false;
                int cycles = 1;
                while (!placed)
                {
                    if (cycles < 20)
                    {
                        index = (index + index - 1) % (tableToEdit.Length - 1);
                    }
                    else
                    {
                        index = (index + 1) % (tableToEdit.Length - 1);
                    }

                    if (tableToEdit[index] == null)
                    {
                        tableToEdit[index] = value;
                        placed = true;
                    }
                    cycles++;
                }
            }
        }

        private int GetHashIndex(string value)
        {
            int index = -1;
            char[] representation = value.ToCharArray();
            foreach (char character in representation)
            {
                index += GetCharHashValue(char.ToLower(character));
            }

            if (index == 0)
            {
                index = 1;
            }
            else if (index < 0)
            {
                index = Math.Abs(index);
            }

            if (resizing)
            {
                index = index % (resizingTable.Length - 1);
            }
            else
            {
                index = index % (table.Length - 1);
            }

            return index;
        }

        private int GetCharHashValue(char character)
        {
            switch (character)
            {
                case 'a':
                    return 1;
                case 'b':
                    return 2;
                case 'c':
                    return 3;
                case 'd':
                    return 4;
                case 'e':
                    return 5;
                case 'f':
                    return 6;
                case 'g':
                    return 7;
                case 'h':
                    return 8;
                case 'i':
                    return 9;
                case 'j':
                    return 10;
                case 'k':
                    return 11;
                case 'l':
                    return 12;
                case 'm':
                    return 13;
                case 'n':
                    return 14;
                case 'o':
                    return 15;
                case 'p':
                    return 16;
                case 'q':
                    return 17;
                case 'r':
                    return 18;
                case 's':
                    return 19;
                case 't':
                    return 20;
                case 'u':
                    return 21;
                case 'v':
                    return 22;
                case 'w':
                    return 23;
                case 'x':
                    return 24;
                case 'y':
                    return 25;
                case 'z':
                    return 26;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case '0':
                    return 10;
                default:
                    return 0;
            }
        }

        private int FindEmptyIndex(int index)
        {
            bool placeFound = false;
            int cycles = 1;
            string[] tableToSearch;
            if (resizing)
            {
                tableToSearch = resizingTable;
            }
            else
            {
                tableToSearch = table;
            }

            while (!placeFound)
            {
                if (cycles < 20)
                {
                    index = (index + index - 1) % (tableToSearch.Length - 1);
                }
                else
                {
                    index = (index + 1) % (tableToSearch.Length - 1);
                }

                if (tableToSearch[index] == null)
                {
                    placeFound = true;
                }
                cycles++;
            }
            return index;
        }

        /// <summary>
        /// Delete a string from the hash table if it exists.
        /// </summary>
        /// <param name="value">The value to be deleted from the table.</param>
        public void Delete(string value)
        {
            KeyValuePair<bool, int> valueProbed = ProbeForValue(value);
            if (valueProbed.Key)
            {
                if (resizing)
                {
                    if (resizingTable[valueProbed.Value] == value)
                    {
                        resizingTable[valueProbed.Value] = null;
                    }
                    if (table[valueProbed.Value] == value)
                    {
                        table[valueProbed.Value] = null;
                    }
                }
                else
                {
                    table[valueProbed.Value] = null;
                }
            }

            Count--;
        }

        /// <summary>
        /// Searches for a string in the hash table. Returns true if the string exists in the table.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns></returns>
        public bool Search(string value)
        {
            KeyValuePair<bool, int> valueProbed = ProbeForValue(value);
            return valueProbed.Key;
        }

        private KeyValuePair<bool, int> ProbeForValue(string value)
        {
            bool found = false;
            int index = GetHashIndex(value);
            int oldIndex = index % (table.Length - 1);
            int cycles = 0;

            while (!found)
            {
                if (resizing)
                {
                    if (table[index] == null && resizingTable[index] == null)
                    {
                        return new KeyValuePair<bool, int>(false, index);
                    }
                    else if (resizingTable[index] == value)
                    {
                        found = true;
                    }
                    else if (table[oldIndex] == value)
                    {
                        found = true;
                        index = oldIndex;
                    }
                    else
                    {
                        if (cycles < 20)
                        {
                            index = (index + index - 1) % (resizingTable.Length - 1);
                            oldIndex = (oldIndex + oldIndex - 1) % (table.Length - 1);
                        }
                        else
                        {
                            index = (index + 1) % (resizingTable.Length - 1);
                            oldIndex = (oldIndex + 1) % (table.Length - 1);
                        }
                    }
                }
                else
                {

                    if (table[index] == null)
                    {
                        return new KeyValuePair<bool, int>(false, index);
                    }
                    else if (table[index] == value)
                    {
                        found = true;
                    }
                    else
                    {
                        if (cycles < 20)
                        {
                            index = (index + index - 1) % (table.Length - 1);
                        }
                        else
                        {
                            index = (index + 1) % (table.Length - 1);
                        }
                    }
                }
                cycles++;
            }
            
            return new KeyValuePair<bool, int>(found, index);
        }
    }
}
