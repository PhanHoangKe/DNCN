
function createSakura() {
    const sakura = document.createElement('div');
    sakura.classList.add('sakura');
    sakura.style.left = Math.random() * window.innerWidth + 'px';
    sakura.style.animationDuration = Math.random() * 7 + 7 + 's';
    document.body.appendChild(sakura);

    setTimeout(() => {
        sakura.remove();
    }, 5000);
}

setInterval(createSakura, 300);
