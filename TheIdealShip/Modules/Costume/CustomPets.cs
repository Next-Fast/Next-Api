/* using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Modules.Costume;

[HarmonyPatch]
public class CustomPets
{
    public class Petinfo
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public string Resource { get; set; }

        public Petinfo
        (
            string author,
            string name,
            string resource
        )
        {
            this.Author = author;
            this.Name = name;
            this.Resource = resource;
        }
    }

    [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetPetById))]
    public static class HatManagerGetPetByIdPatch
    {
        static bool canLoad;
        static bool hasLoad = false;
        static int petId = 0;
        private static List<PetData> allPetsList = new List<PetData>();
    
        public static List<Petinfo> petinfos = new List<Petinfo>();

        public static void Prefix(HatManager __instance)
        {
            allPetsList = __instance.allPets.ToList();
            petinfos.Add(new Petinfo("天寸", "披萨成精", "pisa.png"));
    
            foreach (var i in petinfos)
            {   try
                {
                    PetData pet = CreatePet(i);
                    allPetsList.Add(pet);
                    petId++;
                }
                catch (Exception ex)
                {
                    Exception(ex, filename: "CustomPets");
                    continue;
                }
             }
            hasLoad = true;
            __instance.allPets = allPetsList.ToArray();
        }

        public static void Postfix()
        {
            hasLoad = false;
        }

        private static Sprite CreateHatSprite(string patch)
        {
            Texture2D texture = Helpers.LoadTextureFromResources(patch);

            if (texture == null) return null;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.53f, 0.575f), texture.width * 0.375f);

            if (sprite == null) return null;
            return sprite;
        }

        public static PetData CreatePet(Petinfo info)
        {
            PetData pet = ScriptableObject.CreateInstance<PetData>();

            pet.name = info.Name;
            pet.displayOrder = 10 + petId;
            pet.ProductId = "pet_" + info.Name.Replace(' ', '_');
            pet.BundleId = "pet_" + info.Name.Replace(' ', '_');
            pet.NotInStore = true;
            pet.Free = true;
            if (CreateHatSprite(".\\Resources\\pisa.png") != null)
            {
                pet.viewData.viewData.rend.sprite = CreateHatSprite(".\\Resources\\pisa.png");
            }
            return pet;
        }
    }
} */