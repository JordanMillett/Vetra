function scrollToElementByClass(className)
{
    const element = document.querySelector(`.${className}`);
    if (element)
    {
        window.scrollTo({
            top: element.getBoundingClientRect().top - 20,
            behavior: "smooth"
        });
        
        element.focus();
    }
}

function setDarkTheme(useDark)
{
    const root = document.documentElement;
    if (useDark)
    {
        root.setAttribute("data-theme", "dark");
    } else
    {
        root.removeAttribute("data-theme");
    }
}

function md5Hash(input)
{
    return SparkMD5.hash(input);
}