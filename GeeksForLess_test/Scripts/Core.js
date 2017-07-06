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


/*      Text autosize helper        */

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
            form.classList.add('is-error');
            form.classList.remove('is-uploading');
            errorMsg.textContent = xhr.responseText;
        }
    });
});

/*        Catching reply button click event and add to form id of comment to which we reply         */

$('.btn-reply').click(function () {
    var button = $(this);
    var newComment = $('.new-comment');
    var ReplyTo = $('#Reply_to');
    
    ReplyTo.text(button.data('target'));
    newComment.find('textarea').text('#'+button.data('target') + ', ');
    newComment.find('textarea').focus();
});