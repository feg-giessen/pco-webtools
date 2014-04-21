/// <reference path="jquery-2.1.0.js" />

var liveApp = {
    songs: [],
    items: [],
    serviceDate: null,
    serviceEnd: null,
    serviceTimeOffset: 0,

    elements: {
        clock_current: null,
        clock_pre: null,
        clock_past: null,
        live_tag: null,
        offset_tag: null
    }
};

liveApp.toggleSong = function (songElement, itemid) {
    if (songElement.is(':visible')) {
        songElement.slideUp();
        localStorage.setItem('song_' + itemid, 0);
    } else {
        songElement.slideDown();
        localStorage.removeItem('song_' + itemid);
    }
}

liveApp.hideSongs = function () {    
    $.each(liveApp.songs, function (i, song) {
        if (song.e.is(':visible')) {
            song.e.slideUp();
            localStorage.setItem('song_' + song.itemid, 0);
        }
    });
}

liveApp.showSongs = function () {
    $.each(liveApp.songs, function (i, song) {
        if (!song.e.is(':visible')) {
            song.e.slideDown();
            localStorage.removeItem('song_' + song.itemid);
        }
    });
}

liveApp.init = function () {
    var songElements = $('.song-details');
    $.each(songElements, function (i, songElement) {
        var $songElement = $(songElement);
        var itemtd = $songElement.parents('td');
        var b = itemtd.find('.song-toggle');

        var song = {
            itemid: $songElement.parents('tr').data('id'),
            b: b,
            e: $songElement
        };

        liveApp.songs.push(song);

        b.on('click', function () {
            liveApp.toggleSong($songElement, song.itemid);
        });

        if (localStorage.getItem('song_' + song.itemid) == 0) {
            $songElement.hide();
        }
    });

    var items = $('table.live tbody tr');
    $.each(items, function (i, itemElement) {
        var $itemElement = $(itemElement);
        var item = {
            e: $itemElement,
            offset: parseInt($itemElement.data('time-offset'))
        };

        liveApp.items.push(item);
    });

    liveApp.elements.clock_current = $('.plan-clock .current-clock');
    liveApp.elements.clock_pre = $('.plan-clock .to-service-clock');
    liveApp.elements.clock_past = $('.plan-clock .in-service-clock');
    liveApp.elements.live_tag = $('.live-tag');
    liveApp.elements.offset_tag = $('.offset-tag');
};

liveApp.updateTimes = function () {
    var d = new Date(Date.now());
    var h = d.getHours(); if (h < 10) h = "0" + h;
    var m = d.getMinutes(); if (m < 10) m = "0" + m;
    var s = d.getSeconds(); if (s < 10) s = "0" + s;

    var diff = 0;

    liveApp.elements.clock_current.text(h + ':' + m + ':' + s + ' Uhr');
    if (liveApp.serviceDate) {
        diff = d - liveApp.serviceDate;

        if (diff < 0) {
            // before service
            var diff2 = -1 * diff;
            var hours = Math.round(diff2 / 3600000);
            var days = Math.round(hours / 24);
            if (days > 1) {
                liveApp.elements.clock_pre.text('- ' + days + ' Tage');
            } else if (hours > 1 && hours < 24) {
                liveApp.elements.clock_pre.text('- ' + hours + ' Stunden');                
            } else if (hours < 24) {
                liveApp.elements.clock_pre.text('- ' + Math.round(diff2 / 60000) + ' Minuten');
            }
            liveApp.elements.clock_past.text('');
        } else {
            // in service
            var hours = Math.round(diff / 3600000);
            if (hours > 2 && hours < 24) {
                liveApp.elements.clock_past.text('+ ' + hours + ' Stunden');
            } else if (hours < 24) {
                liveApp.elements.clock_past.text('+ ' + Math.round(diff / 60000) + ' Minuten');
            }
            liveApp.elements.clock_pre.text('');
        }

        var currentLive = null;
        var currentOffset = liveApp.serviceTimeOffset + (diff / 1000);
        $.each(liveApp.items, function (i, item) {
            item.e.removeClass('live');
            if ((currentOffset < 0 && item.offset >= currentOffset)
                || (currentOffset > 0 && item.offset <= currentOffset)) {
                liveApp.elements.live_tag
                    .css('height', (item.e.height() - 1) + 'px')
                    .css('top', (item.e.position().top + 1) + 'px')
                    .show();
                currentLive = item;
            }
        });

        if (currentLive != null) {
            currentLive.e.addClass('live');
        }

        if (liveApp.serviceEnd) {
            if ((serviceDate - d) < 0 && (d - serviceEnd) < 0) {
                liveApp.elements.clock_current.addClass('live');
            } else {
                liveApp.elements.clock_current.removeClass('live');
                liveApp.elements.live_tag.hide();
            }
        }
    }
};

$(function () {
    liveApp.init();
});