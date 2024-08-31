using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamController : MonoBehaviour
{
    public string steamID;
    public string steamName;
    public Texture2D steamAvatar;

    private void Start()
    {
        steamID = SteamUser.GetSteamID().ToString();
        steamName = SteamFriends.GetPersonaName();
        int avatarInt = SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID());
        steamAvatar = GetSteamImageAsTexture(avatarInt);
    }

    private Texture2D GetSteamImageAsTexture(int image)
    {
        bool isValid = SteamUtils.GetImageSize(image, out uint width, out uint height);
        if (!isValid) return null;

        byte[] imageData = new byte[width * height * 4];
        isValid = SteamUtils.GetImageRGBA(image, imageData, (int)(width * height * 4));
        if (!isValid) return null;

        Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
        texture.LoadRawTextureData(imageData);
        texture.Apply();
        return texture;
    }
}
