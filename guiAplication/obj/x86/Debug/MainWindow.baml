   M S B A M L   `   `   `  ����  guiAplication    guiAplication.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  5   	     � Title$  
MainWindow��5   	   $	��892��6   $	��609��6)   .��5      � .+�5   
   �� -   5        $��nameOfStockToSearch��5   M   $��23��6   $
��Left=�6   $��
136,39,0,0q�69   $	��Top=�6h   $	��120��6�   6�   �� 5   
   -   5        $��label1��5   g   $��Wpisz nazwe gieldy��6   $��28��6-   $
��Left=�69   $��	12,39,0,0q�6T   $	��Top=�6u   $	��126��6�   �� 5	   
   -   5        $��downloadDataFromWeb��5	   �   $��Sciagnij dane��6    � ToolTip$R Lsciagne dane ze strony http://www.findata.co.nz/markets/ dla podanej gieldy ��6)   $��23��6�   $
��Left=�6�   $��
76,112,0,0q�6�   $	��Top=�6�   $��75��6�   6�   �� 5
   
   -   5        $��saveDataToDb��5
   �   $��zapisz dane do db��6   $6 0zapisze uprzednio sciagniete dane do bazy danych��6-   $��23��6h   $
��Left=�6t   $��193,112,0,0q�6�   $	��Top=�6�   $	��124��6�   6�   �� 5   
   -   5        $��downloadAndSaveAll��5   �   $8��2sciag i zapisz dane dla wszystkich moliwych spolek��6   $T Nwykonam operacje sciagniecia danych i zapisu dla notowan: NYSE, NASDAQ i AMEX ��6N   $��23��6�   $
��Left=�6�   $��
76,155,0,0q�6�   $	��Top=�6�   $	��291��6  6   �� 5   
   -   5        $��searchRecordsForStock��5   N   $��23��6   $
��Left=�6   $��301,223,0,0q�69   $	��Top=�6l   $	��147��6�   �� 5   
   -   5        $��label2��5   �   $2��,wpisz symbol spolki aby wyswietlic notowanie��6   $��28��6G   $
��Left=�6S   $��
24,221,0,0q�6n   $	��Top=�6�   $	��253��6�   �� 5   
   -   5        $��getRecordsForStock��5   �   $��wyswietl��6   $Z Twyszukam w bazie danych, czy notowanie dla danej spolki zostalo wczesniej sciagniete��6$   $��23��6�   $
��Left=�6�   $��
76,255,0,0q�6�   $	��Top=�6�   $��75��6�   6�   �� 5   
   -	   5        $��label3��5   \   $��Symbol��6   $��28��6!   $
��Left=�6-   $��
76,366,0,0q�6H   $	��Top=�6j   �� 5   
   -
   5        $��label4��5   [   $
��High��6   $��28��6   $
��Left=�6+   $��136,366,0,0q�6F   $	��Top=�6i   �� 5   
   -   5        $��label5��5   Z   $	��Low��6   $��28��6   $
��Left=�6*   $��185,366,0,0q�6E   $	��Top=�6h   �� 5   
   -   5        $��label6��5   \   $��Close��6   $��28��6    $
��Left=�6,   $��238,366,0,0q�6G   $	��Top=�6j   �� 5   
   -   5        $��label7��5   ]   $��Volume��6   $��28��6!   $
��Left=�6-   $��301,366,0,0q�6H   $	��Top=�6k   �� 5   
   -   5        $��label8��5   ]   $��Change��6   $��28��6!   $
��Left=�6-   $��370,366,0,0q�6H   $	��Top=�6k   �� 5   
   -   5        $��
dataResult��5   P   $	��127��6   $
��Left=�6!   $��
84,402,0,0q�6<   $�� ��6b   $	��Top=�6j   $	��421��6�   �� 5   
   -   5        $��label9��5   [   $
��Date��6   $��28��6   $
��Left=�6+   $��443,366,0,0q�6F   $	��Top=�6i   8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  5   
   -   5        $��plotter��5   1   $��76,535,82,131q�6      HorizontalAxis 5      I  CMicrosoft.Research.DynamicDataDisplay.Charts.HorizontalDateTimeAxis  5      -   5        $��dateAxis��5   ,   $��50��6<   $	��403��6H      VerticalAxis 5      ?  9Microsoft.Research.DynamicDataDisplay.Charts.VerticalAxis  5      -   5        $��	countAxis��5   "   3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. 5       2  ,Microsoft.Research.DynamicDataDisplay.Header  $��Arial)�6   $��Stock Information��6+   =  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  5!      $��Arial)�6#   $��Close��66   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  5"      $��Arial)�6%   $
��Date��68   5$      �� 5%   
   -   5        $��button1��5%   f   $��wyczysc wykres��6   $��23��6*   $
��Left=�66   $��388,777,0,0q�6Q   $	��Top=�6u   $	��117��6�   6�   5&      5'      