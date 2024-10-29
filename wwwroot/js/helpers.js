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
