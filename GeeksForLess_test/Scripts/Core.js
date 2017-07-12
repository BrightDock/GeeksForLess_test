$(document).ready(function () {
    changeWindowHeight();
    $('.btn-like').click(function () {
        processLikes(this);
    });
});

$(window).resize(function () {
    changeWindowHeight();
});

function processLikes(button) {
    var button = $(button);
    var commentId = button.data('target');
    $.ajax({
        type: 'post',
        url: button.data('path') + '?ID=' + commentId,
        contentType: false,
        processData: false,
        contentType: 'application/json',
        success: function (result) {
            button.parent().replaceWith(result);
            $('.btn-like').off();
            $('.btn-like').click(function() { processLikes(this) });
        },
        error: function (xhr, status, p3) {

        }
    });
}


/*       Make our document min height full height of window          */

function changeWindowHeight() {
    $('body').css("height", 'auto');
    var documentHieght = $(document).innerHeight();
    var windowHeight = $(window).innerHeight();
    var headHeight = $('.header').innerHeight();
    var footer = $('footer').innerHeight();

    if (windowHeight < documentHieght) {
        $('body').css("height", documentHieght);
    }
    else {
        $('body').css("height", documentHieght - headHeight - 15);
    }
}


/*      Text autosize height        */

function textAreaAutosize(selector) {
    this.txt = $(selector);

    textAreaAutosize.prototype.update = function () {
        this.txt = $(selector);
        var i = 0;
        this.txt.each(function () {
            hiddenDiv = $(document.createElement('div')),
            $copyText = textAreaAutosize.copyText(hiddenDiv, $(this));

            $(this).addClass('noscroll');
            hiddenDiv.addClass('hidden');
            hiddenDiv.attr('id', "hiddenDiv" + i);
            $('body').append(hiddenDiv);

            $(this).bind('focusin keyup', function (e) {
                textAreaAutosize.copyText(hiddenDiv, $(this));
            });
            i++;
        });
    }

    textAreaAutosize.copyText = function(hiddenDiv, txt) {
        hiddenDiv.css("paddind", txt.css("padding"));
        hiddenDiv.css("font", txt.css("font"));
        hiddenDiv.css("width", txt.width());

        hiddenDiv.html(txt.val().replace(/\n/g, '<br />'));

        txt.css('height', hiddenDiv.innerHeight() + 10);

        $(window).trigger('resize', this);
    }
}

var TAAutosize = new textAreaAutosize('.text-area');
TAAutosize.update();

/*      Catch modal window open event and load content for it        */

$('#Modal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget)     // Button that triggered the modal
    var title = button.data('whatever')     // Extract info from data-* attributes
    var MessageId = button.data('bind') // 

    var modal = $(this);
    var message = $('#' + MessageId);
    var MessageText = message.find('.message-text').text(); 
    modal.find('.modal-title').text(title + ' #' + MessageId);
    modal.find('.textarea').val(MessageText);
    
    modal.find('.modal-body').empty();
    modal.find('.modal-body').html('<div class="progress">' +
        '<div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"' + 
        'aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>');
    $.ajax({
        type: 'get',
        url: button.data('path') + '?ID=' + MessageId,
        contentType: false,
        processData: false,
        contentType: 'application/json',
        success: function (result) {
            modal.find('.modal-body').html(result);
            TAAutosize.update();
        },
        error: function (xhr, status, p3) {

        }
    });
});

/*        Catching reply button click event and add to form id of comment to which we reply         */

$('.btn-reply').click(function () {
    var button = $(this);
    var newComment = $('.new-comment');
    var ReplyTo = $('#Reply_to');
    var ReplyToLabel = $('.reply-to-label') == undefined ? $('.reply-to-label') : $(document.createElement('label'));
    ReplyToLabel.addClass('reply-to-label');
    ReplyToLabel.attr('title', 'Отменить');
    newComment.append(ReplyToLabel);
    
    $('html, body').animate({
        scrollTop: newComment.offset().top + ($(window).innerWidth() > 544 ? newComment.innerHeight() : -60)
    }, 500);

    ReplyTo.val(button.data('target'));
    ReplyToLabel.html('<i class="fa fa-reply-all mr-2" aria-hidden="true"></i>#' + button.data('target'));
    newComment.find('textarea').focus();

    ReplyToLabel.click(function () {
        ReplyTo.val('');
        $(this).remove();
    });
});

function sendAjax(url, form, resultBlock) {
    var data = $(form).serialize();

    modal.find('.modal-body').empty();
    modal.find('.modal-body').html('<div class="progress">' +
        '<div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar"' +
        'aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>');

    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (result) {
            $(resultBlock).html(result);
            modal.modal('hide');
            modal.find('.modal-body').empty();
        },
        error: function (xhr, str) {
//            alert('Возникла ошибка: ' + xhr.responseCode);
            modal.modal('hide');
            modal.find('.modal-body').empty();
        }
    });
}