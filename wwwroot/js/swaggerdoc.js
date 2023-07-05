var exten = "";//下載檔案尾碼名
var timerLoadExportBtn = null;
$(document).delegate("#select", "change", function () {
    setTimeout("LoadExportApiWordBtn()", 300);//載入匯出按鈕
    console.log("dom export ok");
});

$(document).ready(function () {
    InitLoad();//初始化匯出
    //loading設置
    $.busyLoadSetup({
        animation: "slide",
        background: "rgba(255, 152, 0, 0.86)"
    });
});

//初始化
function InitLoad() {
    setTimeout("LoadExportApiWordBtn()", 300);//載入匯出按鈕
}

//載入自訂匯出按鈕
function LoadExportApiWordBtn() {
    $(".information-container").height(240);//文檔介紹區域高度
    $(".topbar").height(35);
    var btnExport = "<div class='selectBox' style='position: absolute;margin: 0;padding: 0;margin-left: 1432px;top: 2.5px;'>" +
        "<span><a href='javascript:void(0);'>匯出離線文檔</a></span>" +
        "<div class='drop'>" +
        "<ul style='margin: 0;padding: 0;'>" +
        "<li>" +
        "<a href='javascript:void(0);' onclick='ExportApiWord(1)'>匯出 Word</a>" +
        "</li>" +
        //"<li>" +
        //"<a href='javascript:void(0);' onclick='ExportApiWord(2)'>匯出 PDF</a>" +
        //"</li>" +
        //"<li>" +
        //"<a href='javascript:void(0);' onclick='ExportApiWord(3)'>匯出 Html</a>" +
        //"</li >" +
        //"<li>" +
        //"<a href='javascript:void(0);' onclick='ExportApiWord(4)'>匯出 Xml</a>" +
        //"</li >" +
        //"<li>" +
        //"<a href='javascript:void(0);' onclick='ExportApiWord(5)'>匯出 Svg</a>" +
        //"</li >" +
        "</ul >" +
        "</div >" +
        "</div >";
    //information-container這個元素是swagger後期動態渲染出來的，所有這裡要加個迴圈判斷。
    //第一次進來如果有這個class直接載入按鈕退出
    if ($("*").hasClass("information-container")) {
        $(".information-container").append(btnExport);
        return;
    }
    //沒有元素等待元素出現在載入按鈕
    timerLoadExportBtn = setInterval(function () {
        if ($("*").hasClass("information-container")) {
            $(".information-container").append(btnExport);
            console.log("load ok");
            window.clearInterval(timerLoadExportBtn);
            return;
        }
        console.log("loading");
    }, 788);
}

/**
 * 延遲函數
 * @param {any} delay
 */
function sleep(delay) {
    var start = (new Date()).getTime();
    while ((new Date()).getTime() - start < delay) {
        continue;
    }
}

/*
 * 匯出
 */
function ExportApiWord(type) {
    switch (type) {
        case 1:
            exten = ".docx";
            break;
        case 2:
            exten = ".pdf";
            break;
        case 3:
            exten = ".html";
            break;
        case 4:
            exten = ".xml";
            break;
        case 5:
            exten = ".svg";
            break;
    }
    var version = $("#select option:selected").val();
    version = version.split('/')[2];
    var url = '/api/swagger-doc/ExportWord?type=' + exten + "&version=" + version;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);    // 也可以使用POST方式，根據介面
    xhr.responseType = "blob";  // 返回類型blob
    // 定義請求完成的處理函數，請求前也可以增加載入框/禁用下載按鈕邏輯
    xhr.onload = function () {
        // 請求完成
        const fileName = this.getResponseHeader('content-disposition') === null ? '' : this.getResponseHeader('content-disposition').split(';')[1].split('filename')[1].split('=')[1].trim();
        if (this.status === 200) {
            // 返回200
            var blob = this.response;
            var reader = new FileReader();
            reader.readAsDataURL(blob); // 轉換為base64，可以直接放入a表情href
            reader.onload = function(e) {
                // 轉換完成，創建一個a標籤用於下載
                var a = document.createElement('a');
                a.download = fileName;
                a.href = e.target.result;
                $("body").append(a); // 修復firefox中無法觸發click
                a.click();
                $(a).remove();
            }
        } else {
            alert(this.status+this.statusText);
        }
        //關閉load
        $.busyLoadFull('hide',
            {
                animation: "fade"
            });
    };
    // 發送ajax請求
    xhr.send();
    //打開loader遮罩
    $.busyLoadFull('show', {
        text: "LOADING ...",
        animation: "fade"
    });
}