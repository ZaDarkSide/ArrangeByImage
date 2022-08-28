
Saved from [officewarfare.net](https://web.archive.org/web/20131023181304/http://officewarfare.net/index.php/arrange-by-image/)

---

#### Arrange By Image

---

#### Back story

Many of you may or may not have seen the video [sales guy vs the web dude](http://thewebsiteisdown.com/) but if you haven’t you probably should. Be warned though Not safe for work, there is swearing involved. Well anyways at one point in this video you happen across sales guy’s desktop and you see that his icons are arranged in the shape of a [male genitalia]. The tech who is helping him arranges them alphabetically and when the sale guy complains the tech responds that he cant put it back there is no arrange by [male genitalia]. Well one person on the web thought that there probably should be that feature in windows so he made a call out to windows programmers asking them to write it. As it turns out someone [Taktaal] answered the call, you can see the site in question and download the original source we edited [here](https://web.archive.org/web/20120619032849/http://arrangebypenis.com/). Now my co-worker [Mike] and I thought that was a sweet feature to have in windows and we started digging into the source provided with the ArrangeBy[MaleGenitalia] and noticed that it was written really well and would be easy to modify. Well long story short we modified it and came up with ArrangeByImage, which allows you to choose a black and white bitmap image and arrange your icons to match the black parts of the image.

---

#### Usage

Download the source and program above. Make sure you turn “Align To Grid” off on your desktop (in 1.2 you no longer have to turn Align to Grid off, the app does it for you). Then run the program and browse to any black and white bmp image. Click on arrange by image and enjoy.

When making your own bmp images to feed into the program keep in mind that 1px width black lines work the best.

Further I went ahead and added command line switches so you can run this in command line.

The basic command line would be

```
arrangebyimage -bmp smilly.bmp -silent
```

Here is the usage listing:

```
ArrangeByImage v-1.2 USAGE

ArrangeByImage -bmp pathToImage [-icons XX] [ -silent ] [ -help ] [ -restore ] [ -nosave ]

-bmp the path to the image to be used to arrange the icons

-icons a number, the desired number of icons to sample for if left blank the number of icons on the desktop will be used.

-silent program will automatically arrange icons and close itself displaying nothing to the user.

-nosave by default the current icon layout is saved, this flag will stop that behavior.

-restore restores a saved version of the layout (undoes what you have wrought).

-help display this menu

-smiley arrange icons in the shape of a smiley (built in image)

-heart arrange icons in the shape of a heart (built in image)

-star arrange icons in the shape of a star (built in image)

-penis arrange icons in the shape of a penis (built in image, hidden option)
```

---

#### Screen Shots

![Heart](https://web.archive.org/web/20131023181304im_/http://officewarfare.net/wp-content/uploads/2009/05/heart.jpg)
![Smilley](https://web.archive.org/web/20131023181304im_/http://officewarfare.net/wp-content/uploads/2009/05/smilly.jpg)
