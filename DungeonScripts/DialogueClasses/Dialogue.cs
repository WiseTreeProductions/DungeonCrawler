using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Page> pages;
    public Page currentPage;
    public int finalPageIndex = -1;

    public Page GetPageWithIndex(int index)
    {
        foreach (Page page in pages)
        {
            if (page.index == index)
            {
                return page;
            }
        }

        return null;
    }

    public void SetNextPage()
    {
        currentPage = GetPageWithIndex(currentPage.nextPageIndex);
    }

    public void SetPageByID(int ID)
    {
        currentPage = GetPageWithIndex(ID);
    }


}
