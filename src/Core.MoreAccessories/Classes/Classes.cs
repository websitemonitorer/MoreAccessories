﻿using System.Collections.Generic;
#if EC
using HPlay;
using ADVPart.Manipulate;
using ADVPart.Manipulate.Chara;
#endif
#if KK || KKS
#endif
using System.Linq;

namespace MoreAccessoriesKOI
{
    #region Private Types
    public class CharAdditionalData
    {
        public CharAdditionalData() { }

#if KKS || KK
        public CharAdditionalData(ChaControl chactrl)
        {
            nowAccessories = chactrl.nowCoordinate.accessory.parts.ToList();
            nowAccessories.RemoveRange(0, 20);
            for (var i = 0; i < chactrl.chaFile.coordinate.Length; i++)
            {
                rawAccessoriesInfos[i] = chactrl.chaFile.coordinate[i].accessory.parts.ToList();
                rawAccessoriesInfos[i].RemoveRange(0, 20);
            }
        }
#elif EC
            public CharAdditionalData(ChaControl chactrl)
            {
                nowAccessories = chactrl.nowCoordinate.accessory.parts.ToList();
                nowAccessories.RemoveRange(0, 20);

                rawAccessoriesInfos[0] = chactrl.chaFile.coordinate.accessory.parts.ToList();
                rawAccessoriesInfos[0].RemoveRange(0, 20);
            }
#endif
        public CharAdditionalData(ChaFileAccessory.PartsInfo[] parts)
        {
            nowAccessories = parts.ToList();
            nowAccessories.RemoveRange(0, 20);
        }

        public List<ChaFileAccessory.PartsInfo> nowAccessories = new List<ChaFileAccessory.PartsInfo>();
        internal List<bool> showAccessories = new List<bool>();

#if EC
            public List<int> advState = new List<int>();
#endif
        public readonly Dictionary<int, List<ChaFileAccessory.PartsInfo>> rawAccessoriesInfos = new Dictionary<int, List<ChaFileAccessory.PartsInfo>>();
    }

#if KK || KKS

#elif EC
        private class PlaySceneSlotData
        {
            public RectTransform slot;
            public TextMeshProUGUI text;
            public Button button;
        }

        private class ADVSceneSlotData
        {
            public RectTransform slot;
            public TextMeshProUGUI text;
            public Toggle keep;
            public Toggle wear;
            public Toggle takeOff;
        }
#endif
    #endregion
}