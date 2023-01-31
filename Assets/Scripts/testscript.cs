using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testscript : MonoBehaviour
{
    public TextAsset textAssetData;

    void Start()
    {

        Load(textAssetData);

    }

	public class Row
	{
		public string Name;
		public string Dialogue;
		public string PropSprite;
		public string CharacterSprite;

	}

	List<Row> rowList = new List<Row>();
	bool isLoaded = false;

	public bool IsLoaded()
	{
		return isLoaded;
	}

	public List<Row> GetRowList()
	{
		return rowList;
	}

	public void Load(TextAsset csv)
	{
		rowList.Clear();
		string[][] grid = CsvParser2.Parse(csv.text);
		for(int i = 1 ; i < grid.Length ; i++)
		{
			Row row = new Row();
			row.Name = grid[i][0];
			row.Dialogue = grid[i][1];
			row.PropSprite = grid[i][2];
			row.CharacterSprite = grid[i][3];

			rowList.Add(row);
		}
		isLoaded = true;
	}

	public int NumRows()
	{
		return rowList.Count;
	}

	public Row GetAt(int i)
	{
		if(rowList.Count <= i)
			return null;
		return rowList[i];
	}

	public Row Find_Name(string find)
	{
		return rowList.Find(x => x.Name == find);
	}
	public List<Row> FindAll_Name(string find)
	{
		return rowList.FindAll(x => x.Name == find);
	}
	public Row Find_Dialogue(string find)
	{
		return rowList.Find(x => x.Dialogue == find);
	}
	public List<Row> FindAll_Dialogue(string find)
	{
		return rowList.FindAll(x => x.Dialogue == find);
	}
	public Row Find_PropSprite(string find)
	{
		return rowList.Find(x => x.PropSprite == find);
	}
	public List<Row> FindAll_PropSprite(string find)
	{
		return rowList.FindAll(x => x.PropSprite == find);
	}
	public Row Find_CharacterSprite(string find)
	{
		return rowList.Find(x => x.CharacterSprite == find);
	}
	public List<Row> FindAll_CharacterSprite(string find)
	{
		return rowList.FindAll(x => x.CharacterSprite == find);
	}

}