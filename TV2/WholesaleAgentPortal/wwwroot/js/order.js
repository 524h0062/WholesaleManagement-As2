'use strict';
(() => {
    const tbody = document.getElementById('order-rows');
    const add = document.getElementById('add-line');
    const template = document.getElementById('order-line-template');
    const total = document.getElementById('order-total');
    const lineCount = document.getElementById('line-count');
    const form = document.getElementById('order-form');
    if (!tbody || !add || !template || !form) return;
    const number = new Intl.NumberFormat('vi-VN', { maximumFractionDigits: 0 });

    const update = () => {
        const rows = [...tbody.querySelectorAll('.order-row')];
        let sum = 0;
        rows.forEach((row, index) => {
            const select = row.querySelector('.product-select');
            const quantity = row.querySelector('.quantity-input');
            const opt = select.options[select.selectedIndex];
            const price = Number(opt?.dataset.price || 0);
            const stock = Number(opt?.dataset.stock || 0);
            const qty = Number(quantity.value || 0);
            const subtotal = qty > 0 ? price * qty : 0;
            row.querySelector('.price-cell').textContent = price ? number.format(price) + ' VND' : '—';
            row.querySelector('.stock-cell').textContent = opt?.dataset.stock !== undefined ? number.format(stock) : '—';
            row.querySelector('.subtotal-cell').textContent = price && qty ? number.format(subtotal) + ' VND' : '—';
            select.name = `Lines[${index}].ItemId`;
            select.id = `item-${index}`;
            quantity.name = `Lines[${index}].Quantity`;
            quantity.id = `qty-${index}`;
            row.querySelector('.remove-row').disabled = rows.length === 1;
            sum += subtotal;
        });
        total.textContent = number.format(sum) + ' VND';
        lineCount.textContent = rows.length + (rows.length === 1 ? ' line' : ' lines');
        add.disabled = rows.length >= 20;
    };

    tbody.addEventListener('input', update);
    tbody.addEventListener('change', update);
    tbody.addEventListener('click', event => {
        if (!event.target.classList.contains('remove-row')) return;
        if (tbody.querySelectorAll('.order-row').length > 1) {
            event.target.closest('.order-row').remove();
            update();
        }
    });
    add.addEventListener('click', () => {
        if (tbody.querySelectorAll('.order-row').length >= 20) return;
        tbody.appendChild(template.content.cloneNode(true));
        update();
    });
    form.addEventListener('submit', () => {
        if (!form.checkValidity()) return;
        form.querySelector('button[type="submit"]').disabled = true;
    });
    update();
})();
