﻿// Decompiled with JetBrains decompiler
// Type: CreatorModAPI.PillarsDialog
// Assembly: CreatorMod_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B7D80CF5-3F89-46A6-B943-D040364C2CEC
// Assembly location: D:\Users\12464\Desktop\sc2\css\CreatorMod_Android.dll

/*柱子生成*/
/*namespace CreatorModAPI-=  public class PillarsDialog : InterfaceDialog*/
using Engine;
using Game;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CreatorModAPI
{
    public class PillarsDialog : InterfaceDialog
    {
        public SliderWidget Radius;
        public SliderWidget Height;
        public LabelWidget radiusDelayLabel;
        public LabelWidget heightDelayLabel;
        public ButtonWidget SoildButton;
        public ButtonWidget HollowButton;
        public bool typeBool = true;

        public PillarsDialog(CreatorAPI creatorAPI)
          : base(creatorAPI)
        {
            XElement node = ContentManager.Get<XElement>("Dialog/Column");

            LoadChildren(this, node);
            GeneralSet();
            setShaftXYZ();
            X_Shaft.Text = CreatorMain.Display_Key_Dialog("pilldialog1");
            Y_Shaft.Text = "+" + CreatorMain.Display_Key_Dialog("pilldialog2");
            Z_Shaft.Text = CreatorMain.Display_Key_Dialog("pilldialog3");
            Children.Find<LabelWidget>("Name").Text = CreatorMain.Display_Key_Dialog("pilldialog4");
            Radius = Children.Find<SliderWidget>("Slider1");
            radiusDelayLabel = Children.Find<LabelWidget>("Slider data1");
            Height = Children.Find<SliderWidget>("Slider2");
            heightDelayLabel = Children.Find<LabelWidget>("Slider data2");
            Children.Find<StackPanelWidget>("Data3").IsVisible = false;
            SoildButton = Children.Find<ButtonWidget>("Solid");
            SoildButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Column", "Solid");
            HollowButton = Children.Find<ButtonWidget>("Hollow");
            HollowButton.Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Column", "Hollow");
            Children.Find<ButtonWidget>("Cancel").Text = CreatorMain.Display_Key_UI(CreatorAPI.Language.ToString(), "Column", "Cancel");
            LoadProperties(this, node);
        }

        public override void Update()
        {
            base.Update();
            radiusDelayLabel.Text = string.Format(CreatorMain.Display_Key_Dialog("pilldialogr"), (int)Radius.Value);
            heightDelayLabel.Text = string.Format(CreatorMain.Display_Key_Dialog("pilldialogl"), (int)Height.Value);

            upDataButton();
            upClickButton(blockIconWidget.Value);
        }

        public virtual void upClickButton(int id)
        {
            if (SoildButton.IsClicked)
            {
                Task.Run(() =>
               {
                   ChunkData chunkData = new ChunkData(creatorAPI);
                   creatorAPI.revokeData = new ChunkData(creatorAPI);
                   int num = 0;
                   foreach (Point3 pillar in creatorAPI.creatorGenerationAlgorithm.Pillars(creatorAPI.Position[0], (int)Radius.Value, (int)Height.Value, createType, typeBool))
                   {
                       creatorAPI.CreateBlock(pillar, id, chunkData);
                       ++num;
                       if (!creatorAPI.launch)
                       {
                           return;
                       }
                   }
                   chunkData.Render();
                   player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, true, true);

               });
                DialogsManager.HideDialog(this);
            }
            if (!HollowButton.IsClicked)
            {
                return;
            }

            Task.Run(() =>
           {
               ChunkData chunkData = new ChunkData(creatorAPI);
               creatorAPI.revokeData = new ChunkData(creatorAPI);
               int num = 0;
               foreach (Point3 pillar in creatorAPI.creatorGenerationAlgorithm.Pillars(creatorAPI.Position[0], (int)Radius.Value, (int)Height.Value, createType, typeBool, true))
               {
                   creatorAPI.CreateBlock(pillar, id, chunkData);
                   ++num;
                   if (!creatorAPI.launch)
                   {
                       return;
                   }
               }
               player.ComponentGui.DisplaySmallMessage(string.Format(CreatorMain.Display_Key_Dialog("filldialog1"), num), Color.LightYellow, true, true);

           });
            DialogsManager.HideDialog(this);
        }

        public override void upDataButton(CreatorMain.CreateType createType, ButtonWidget button)
        {
            if (this.createType == createType)
            {
                switch (typeBool)
                {
                    case true:
                        typeBool = false;
                        button.Text = "- " + getTypeName(createType) + CreatorMain.Display_Key_Dialog("pilldialogx");
                        button.Color = Color.Red;
                        break;
                    case false:
                        typeBool = true;
                        button.Text = "+ " + getTypeName(createType) + CreatorMain.Display_Key_Dialog("pilldialogx");
                        button.Color = Color.Green;
                        break;
                }

            }
            else
            {
                //点击的按钮不是上次的
                typeBool = true;
                this.createType = createType;
                button.Text = "+ " + getTypeName(createType) + CreatorMain.Display_Key_Dialog("pilldialogx");
                button.Color = Color.Green;
                if (X_Shaft != button)
                {
                    X_Shaft.Text = CreatorMain.Display_Key_Dialog("pilldialogx1");
                    X_Shaft.Color = Color.White;
                }
                if (Y_Shaft != button)
                {
                    Y_Shaft.Text = CreatorMain.Display_Key_Dialog("pilldialogx2");
                    Y_Shaft.Color = Color.White;
                }
                if (Z_Shaft != button)
                {
                    Z_Shaft.Text = CreatorMain.Display_Key_Dialog("pilldialogx3");
                    Z_Shaft.Color = Color.White;
                }
                //  return;


            }
        }
    }
}
