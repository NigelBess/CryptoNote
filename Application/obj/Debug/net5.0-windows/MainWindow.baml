   M S B A M L   `   `   `  ����F  BApplication, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null)clr-namespace:ApplicationApplication  5   	       Application.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Application     5   	     � Title$  
CryptoNote��5   	   $	��450��6   $	��800��6)    J� InputBindings 5
      �� 5   
    �� 	Modifiers$
 Ctrlf�6.   
 �� Key$ S��6@    �� Command 6   �� *Save+6@   �� 5   
   $
 Ctrlf�6.   $ O��6@    6   �� *Open+6@   �� 5   
   $
 Ctrlf�63   $ N��6E    6   �� *	CreateNew+6E   �� 5   
   $
 Ctrlf�6.   $ L��6@    6   �� *Lock+6@   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   o� .@�5      m� $
��File��6   .@�5      m� $	��New��6    m� InputGestureText$ Ctrl+N��6F    m� Command 6(   �� *	CreateNew+6F   m� 5      $
��Open��6   $ Ctrl+O��6C    6)   �� *Open+6C   m� 5      $
��Save��6   $ Ctrl+S��6B    6)   �� *Save+6B   m� 5      $��Save As��6    6,   �� *SaveAs+5      m� 5      $��Security��6   .@�5      m� $��Change Password��6    64   �� *ChangePassword+m� 5       $
��Lock��6   $ Ctrl+L��6C    6)   �� *Lock+6C   5!      m� 5"      $��Tools��6   .@�5#      m� $��Settings��6    6-   �� *OpenSettingsWindow+5$      5%      � 5&   
   $��1��6   $��5q�6   .+�5(      �� .@�5)      �� ��6   �� *
FileName+.��5*      � .+�5+      �� $
��Auto=�6'   $
��Auto=�6J   y�6o   �� *TextVisibility+.��5,      ��  �� AcceptsReturn$
 True��6d   ��6&   �� *	Message+	 �� UpdateSourceTrigger$	 PropertyChanged=�6|   5-      #   Application.ValidationControl  5.      y�62   �� *ValidationVisibility+5/      51      52      54      55      56      