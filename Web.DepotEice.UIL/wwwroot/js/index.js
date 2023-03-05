function getDOMElementPosition(sId) {
    var oElement = document.getElementById(sId);
    var oPosition = new Object();
    oPosition.x = 0;
    oPosition.y = 0;
    if (oElement) {
        oPosition.x = oElement.offsetLeft;
        oPosition.y = oElement.offsetTop;
        var oParent = oElement.offsetParent;
        while (oParent) {
            oPosition.x += oParent.offsetLeft;
            oPosition.y += oParent.offsetTop;
            oParent = oParent.offsetParent;
        }
    }
    return oPosition;
}

function getDOMElementSize(sId) {
    var oElement = document.getElementById(sId);
    var oSize = new Object();
    oSize.width = 0;
    oSize.height = 0;
    if (oElement) {
        const oRect = oElement.getBoundingClientRect();
        oSize.width = oRect.width;
        oSize.height = oRect.height;
    }
    return oSize;
}

function setElementPosition(elementId, x, y) {
    var element = document.getElementById(elementId);
    element.style.left = x + 'px';
    element.style.top = y + 'px';
}

/**
 * Apply a style to an element in the DOM
 * @param {any} oStyleOperation A JSON object with the following properties: elementId, styleName, styleValue
 */
function applyDOMElementStyle(oStyleOperation) {
    var oElement = document.getElementById(oStyleOperation.elementId);
    if (oElement) {
        oElement.style[oStyleOperation.styleName] = oStyleOperation.styleValue;
    }
}