   M S B A M L   `   `   `  ����H  DpedometerForm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:gesturegesture  5   
       pedometerForm.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.4.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  5   
   cclr-namespace:gesture  5   
     � Title$  
MainWindow��5   	   $	��377��6   $	��643��6)   .��5      � -   5         �Name$ 
LayoutRoot��5       � ColumnDefinitions 5   
   �� 5	      $
��137* �6   �� 5
      $
��484* �6   .+�5   
      gesture.skeleton  -   5          Name$ SkeletonViewerElement��5      $��0,192,53,48q�64   $y�Hidden=�6I   �� 5   
   -   5         ��Name$ GameBoardElement��5      $��0,192,53,48q�6+   6B   �� 5   
   -   5        $��androidDeviceImg��5   J   $��98��6   $
��Left=�6   $��	90,57,0,0q�67   $
��Fill=�6b   $	��Top=�6q   $	��132��6�   $.��(/pedometerForm;component/Img/android.jpg��6�   $��2��6�   �� 5   
   -   5        $��kinectDeviceImg��5   K   $��98��6   $
��Left=�6   $��
90,192,0,0q�67   $
��Fill=�6b   $	��Top=�6q   $	��132��6�   $-��'/pedometerForm;component/Img/kinect.jpg��6�   $��2��6�   �� 5   
   -   5        $��label1��5   ]   $��Devices��6   $��28��6"   $
��Left=�6.   $��
130,23,0,0q�6I   $	��Top=�6k   $��78��6�   $��2��6�   �� 5   
   -   5        $��label2��5   \   $��States��6   $��28��6!   $
��Left=�6-   $��
262,23,0,0q�6H   $	��Top=�6j   $��62��6�   $��1��6�   �� 5   
   -   5        $��
textBlock1��5   O   $��26��6   $
��Left=�6    $��
39,311,0,0q�6;   $��	TextBlock��6a   $	��Top=�6r   $	��538��6�   $
��Wrap=�6�   $��2��6�   �� 5   
   -	   5        $��androidDeviceStates��5   ]   $��43��6   $
��Left=�6    $��
196,80,0,0q�6;   $��30(�6O   $��Disconnected��6x   $	��Top=�6�   $	��202��6�   $
��Bold"�6�   $��Normal&�6�   ���	#FFF50000$��1��6�   �� 5   
   -
   5        $��kinectDeviceStates��5   ^   $��43��6   $
��Left=�6    $��196,216,0,0q�6;   $��30(�6P   $��Disconnected��6x   $	��Top=�6�   $	��202��6�   $
��Bold"�6�   
���  ��$��1��6�   �� 5   
   -   5        $��startBtn��5   ]   $��Start��6   $��23��6!   $
��Left=�6-   $��375,282,0,0q�6H   $	��Top=�6m   $��75��6�   {�.  $��1��6�   o� 5   
   -   5        $��menu1��5   6   $��23��6   $
��Left=�6   $	��Top=�6C   $	��621��6[   $��2��6g   .@�5      m� $��Settings��6   .@�5      m� -   5        $��Google Earth��5      61   m� 5      -   5        $��Kinect��5      6+   5      5      3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  5   
   -   5          Name$ MyNotifyIcon��5         
IconSource$ /Img/icon_.ico��5         ToolTipText$ 	pedometer��5      $y�Hidden=�5          MenuActivation$ 
RightClick=�5!      	 � ContextMenu	 5#      �� 5$      .@�5%      m� -   5        $
��Stop��5%      6-   5&      5)      5,      5-      