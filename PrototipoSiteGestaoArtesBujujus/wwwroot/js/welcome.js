const slides = document.querySelectorAll('.slide');
const pontos = document.querySelectorAll('.ponto');
const btnAnterior = document.getElementById('seta-anterior');
const btnProxima = document.getElementById('seta-proxima');
const carrossel = document.querySelector('.carrossel');

let atual = 0;
let temporizador;

function mostrar(indice) {
    // dá a volta: depois do último vai pro primeiro, e antes do primeiro vai pro último
    atual = (indice + slides.length) % slides.length;

    // apaga o "ativo" de todo mundo...
    slides.forEach(slide => slide.classList.remove('ativo'));
    pontos.forEach(ponto => ponto.classList.remove('ativo'));

    // ...e acende só o escolhido
    slides[atual].classList.add('ativo');
    pontos[atual].classList.add('ativo');
}

function iniciarAutomatico() {
    pararAutomatico();
    temporizador = setInterval(() => mostrar(atual + 1), 5000);
}

function pararAutomatico() {
    clearInterval(temporizador);
}

// setas
btnAnterior.addEventListener('click', () => {
    mostrar(atual - 1);
    iniciarAutomatico();
});

btnProxima.addEventListener('click', () => {
    mostrar(atual + 1);
    iniciarAutomatico();
});

// bolinhas: clicar na bolinha N mostra a imagem N
pontos.forEach((ponto, i) => {
    ponto.addEventListener('click', () => {
        mostrar(i);
        iniciarAutomatico();
    });
});

// pausa a troca automática enquanto o mouse está em cima do carrossel
carrossel.addEventListener('mouseenter', pararAutomatico);
carrossel.addEventListener('mouseleave', iniciarAutomatico);

iniciarAutomatico();