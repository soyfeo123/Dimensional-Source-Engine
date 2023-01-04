{
	".": "///////////////////////////////",
	"..": "clienscheme.res for DSE 0.1",
	"...": "//////////////////////////////",
	"....": "clientscheme.res contains style information to be used in DSE's UI.",

	"......": "Fonts are searched inside resource/fonts/, and if they're are not found, fonts are searched inside C:/Windows/Fonts/. Note that you have to put the EXACT file name in, including for fonts inside Windows' Fonts directory. (ex: Segoe UI is bad, segoeui.ttf is good.)",
	".......": "Keep in mind that if fonts are not found, the whole game crashes. And that not every user has a font installed, VCR is one of them.",
	"Fonts": [
		{
			"Name": "Buttons",
			"FontPath": "verdana.ttf",
			"FontSize": 19
		},
		{
			"Name": "GameTitle",
			"FontPath": "NotoSans-Bold.ttf",
			"FontSize": 60
		}
	],

	".....": "Colors are from 0 to 1, not 0 to 255! Keep that in mind.",
	"Colors": [
		{
			"Name": "BtnNeutral",
			"R": 1,
			"G": 1,
			"B": 1
		},
		{
			"Name": "BtnHover",
			"R": 0.7,
			"G": 0.7,
			"B": 0.7
		}
	],

	"Spacing": {
		"MainMenuBtnXMargin": 50
	}
}