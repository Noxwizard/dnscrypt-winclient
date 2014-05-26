using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dnscrypt_winclient
{
	/// <summary>
	/// A simple CSV reader
	/// It is not intended to be fully compliant with RFC 4180 as I do not currently
	/// have a need for supporting escaped quotes or linefeeds in a record.
	/// </summary>
	class CSVReader : IEnumerable<CSVRow>
	{
		private StreamReader reader = null;
		private bool hasHeader = false;
		private List<string> header = null;
		public Dictionary<string, int> headerMap { get; internal set; }
		private List<CSVRow> rows = null;

		public int Count { get;	internal set; }

		public CSVReader(string fileName, bool hasHeader)
		{
			reader = new StreamReader(fileName);
			this.hasHeader = hasHeader;
			this.rows = getRecords();
		}

		/// <summary>
		/// Gets all records in the CSV
		/// </summary>
		/// <returns>A list of all the rows with their records</returns>
		private List<CSVRow> getRecords()
		{
			List<CSVRow> records = new List<CSVRow>();
			string line = null;
			bool parsedHeader = false;
			while ((line = reader.ReadLine()) != null)
			{
				// Skip the header row
				if (hasHeader && !parsedHeader)
				{
					header = parseRow(line).ToList();

					// Set up the map of header name to index
					headerMap = new Dictionary<string, int>(header.Count);
					for (int i = 0; i < header.Count; i++)
					{
						headerMap.Add(header[i], i);
					}
					parsedHeader = true;
					continue;
				}

				records.Add(parseRow(line));
				Count++;
			}

			return records;
		}

		/// <summary>
		/// Parses a row in the CSV
		/// </summary>
		/// <param name="line">The string representation of the row</param>
		/// <returns></returns>
		private CSVRow parseRow(string line)
		{
			CSVRow records = new CSVRow(this);

			line = line.Trim();

			bool quotedRecord = false;
			string currentRecord = "";
			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				// Handle quoted records
				if (c == '"')
				{
					// End of the record
					if (quotedRecord)
					{
						quotedRecord = false;
						records.Add(currentRecord);
						currentRecord = "";
						i++; // Skip over the following comma
					}
					else
					{
						quotedRecord = true;
					}
					continue;
				}

				// End of record
				if (c == ',' && !quotedRecord)
				{
					records.Add(currentRecord);
					currentRecord = "";
					continue;
				}

				// Content of the record
				currentRecord += c;
			}

			return records;
		}

		/// <summary>
		/// Allow accessing this object like an array
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public CSVRow this[int index]
		{
			get
			{
				return this.rows[index];
			}
		}

		/// <summary>
		/// Foreach support
		/// </summary>
		/// <returns></returns>
		public IEnumerator<CSVRow> GetEnumerator()
		{
			return rows.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	/// <summary>
	/// Helper class to facilitate array indexing into the row
	/// </summary>
	class CSVRow
	{
		CSVReader reader;
		List<string> records = new List<string>();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="r">An instance of a CSVReader, for header information</param>
		public CSVRow(CSVReader r)
		{
			reader = r;
		}

		/// <summary>
		/// Inserts a new entry into the list
		/// </summary>
		/// <param name="record">The record to insert</param>
		public void Add(string record)
		{
			records.Add(record);
		}

		/// <summary>
		/// Allow accessing this object like an array
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string this[int index]
		{
			get
			{
				return records[index];
			}

			set
			{
				records[index] = value;
			}
		}

		/// <summary>
		/// Associative indexing
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string this[string index]
		{
			get
			{
				if (reader.headerMap.Count == 0)
				{
					throw new Exception("No header row found. Cannot access via associative index.");
				}

				return records[reader.headerMap[index]];
			}

			set
			{
				if (reader.headerMap.Count == 0)
				{
					throw new Exception("No header row found. Cannot access via associative index.");
				}

				records[reader.headerMap[index]] = value;
			}
		}

		/// <summary>
		/// Conversion to a List object
		/// </summary>
		/// <returns></returns>
		public List<string> ToList()
		{
			return this.records;
		}
	}
}
