using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class currentBGM : MonoBehaviour
{
    public AudioClip clip;
    public int index;
    public Color backgroundColor;
    public GameObject TitleSpace;
    public GameObject ArtistSpace;
    public GameObject AlbumSpace;
    // Start is called before the first frame update
    void Start()
    {
        TitleSpace = transform.Find("name").gameObject;
        ArtistSpace = transform.Find("artist").gameObject;
        AlbumSpace = transform.Find("album").gameObject;
        TitleSpace.GetComponent<TextMeshProUGUI>().text = AudioManager.audioManager.BGMSound[index].name;
        ArtistSpace.GetComponent<TextMeshProUGUI>().text = AudioManager.audioManager.BGMSound[index].artist;
        clip = AudioManager.audioManager.BGMSound[index].clip;
        AlbumSpace.GetComponent<Image>().sprite = AudioManager.audioManager.BGMSound[index].albumCover;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
