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
            int index;
            
            if (resizing || Count >= table.Length * .75) //If we're resizing, we move three values over from the original table to the new table.
            {
                if (!resizing)
                {
                    resizing = true;
                    resizingTable = new string[table.Length * 2];
                }

                bool stop = false;
                int x = 0;
                while (!stop)
                {
                    if (table[resizeIndex] != null)
                    {
                        index = GetHashIndex(table[resizeIndex]);
                        PlaceInTable(table[resizeIndex], index, resizingTable);
                    }
                    resizeIndex++;
                    if (resizeIndex >= table.Length)
                    {
                        resizing = false;
                        resizeIndex = 0;
                        table = null;
                        table = resizingTable;
                        stop = true;
                    }

                    x++;
                    if(x > 2)
                    {
                        stop = true;
                    }
                }

                index = GetHashIndex(newValue);
                PlaceInTable(newValue, index, resizingTable);
            }
            else
            {
                index = GetHashIndex(newValue);
                PlaceInTable(newValue, index, table);
            }

            Count++;
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
            int index;
            switch (character)
            {
                case 'a':
                    index = 1;
                    break;
                case 'b':
                    index = 2;
                    break;
                case 'c':
                    index = 3;
                    break;
                case 'd':
                    index = 4;
                    break;
                case 'e':
                    index = 5;
                    break;
                case 'f':
                    index = 6;
                    break;
                case 'g':
                    index = 7;
                    break;
                case 'h':
                    index = 8;
                    break;
                case 'i':
                    index = 9;
                    break;
                case 'j':
                    index = 10;
                    break;
                case 'k':
                    index = 11;
                    break;
                case 'l':
                    index = 12;
                    break;
                case 'm':
                    index = 13;
                    break;
                case 'n':
                    index = 14;
                    break;
                case 'o':
                    index = 15;
                    break;
                case 'p':
                    index = 16;
                    break;
                case 'q':
                    index = 17;
                    break;
                case 'r':
                    index = 18;
                    break;
                case 's':
                    index = 19;
                    break;
                case 't':
                    index = 20;
                    break;
                case 'u':
                    index = 21;
                    break;
                case 'v':
                    index = 22;
                    break;
                case 'w':
                    index = 23;
                    break;
                case 'x':
                    index = 24;
                    break;
                case 'y':
                    index = 25;
                    break;
                case 'z':
                    index = 26;
                    break;
                case '1':
                    index = 1;
                    break;
                case '2':
                    index = 2;
                    break;
                case '3':
                    index = 3;
                    break;
                case '4':
                    index = 4;
                    break;
                case '5':
                    index = 5;
                    break;
                case '6':
                    index = 6;
                    break;
                case '7':
                    index = 7;
                    break;
                case '8':
                    index = 8;
                    break;
                case '9':
                    index = 9;
                    break;
                case '0':
                    index = 10;
                    break;
                default:
                    index = 0;
                    break;
            }

            return index;
        }

        private void PlaceInTable(string value, int index, string[] tableToEdit)
        {
            if (tableToEdit[index] == null)
            {
                tableToEdit[index] = value;
            }
            else
            {
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
