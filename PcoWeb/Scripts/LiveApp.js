/// <reference path="typings/jquery/jquery.d.ts" />

var LiveApp = (function () {
    function LiveApp() {
        this.songs = [];
        this.items = [];
        this.elements = {
            clock_current: null,
            clock_pre: null,
            clock_past: null,
            live_tag: null,
            offset_tag: null
        };
        this.serviceTimeOffset = 0;
    }
    LiveApp.prototype.toggleSong = function (songElement, itemid) {
        if (songElement.is(':visible')) {
            songElement.slideUp();
            localStorage.setItem('song_' + itemid, "0");
        } else {
            songElement.slideDown();
            localStorage.removeItem('song_' + itemid);
        }
    };

    LiveApp.prototype.hideSongs = function () {
        $.each(this.songs, function (i, song) {
            if (song.e.is(':visible')) {
                song.e.slideUp();
                localStorage.setItem('song_' + song.itemid, "0");
            }
        });
    };

    LiveApp.prototype.showSongs = function () {
        $.each(this.songs, function (i, song) {
            if (!song.e.is(':visible')) {
                song.e.slideDown();
                localStorage.removeItem('song_' + song.itemid);
            }
        });
    };

    LiveApp.prototype.init = function () {
        var _this = this;
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

            _this.songs.push(song);

            b.on('click', function () {
                _this.toggleSong($songElement, song.itemid);
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

            _this.items.push(item);
        });

        this.elements.clock_current = $('.plan-clock .current-clock');
        this.elements.clock_pre = $('.plan-clock .to-service-clock');
        this.elements.clock_past = $('.plan-clock .in-service-clock');
        this.elements.live_tag = $('.live-tag');
        this.elements.offset_tag = $('.offset-tag');
    };

    LiveApp.prototype.updateTimes = function () {
        var _this = this;
        var d = new Date(Date.now());
        var h = d.getHours();
        var m = d.getMinutes();
        var s = d.getSeconds();

        var hours;
        var diff = 0;

        this.elements.clock_current.text((h < 10 ? '0' + h : h.toString()) + ':' + (m < 10 ? '0' + m : m.toString()) + ':' + (s < 10 ? '0' + s : s.toString()) + ' Uhr');
        if (this.serviceDate) {
            diff = d.valueOf() - this.serviceDate.valueOf();

            if (diff < 0) {
                // before service
                var diff2 = -1 * diff;
                hours = Math.round(diff2 / 3600000);
                var days = Math.round(hours / 24);
                if (days > 1) {
                    this.elements.clock_pre.text('- ' + days + ' Tage');
                } else if (hours > 1 && hours < 24) {
                    this.elements.clock_pre.text('- ' + hours + ' Stunden');
                } else if (hours < 24) {
                    this.elements.clock_pre.text('- ' + Math.round(diff2 / 60000) + ' Minuten');
                }
                this.elements.clock_past.text('');
            } else {
                // in service
                hours = Math.round(diff / 3600000);
                if (hours > 2 && hours < 24) {
                    this.elements.clock_past.text('+ ' + hours + ' Stunden');
                } else if (hours < 24) {
                    this.elements.clock_past.text('+ ' + Math.round(diff / 60000) + ' Minuten');
                }
                this.elements.clock_pre.text('');
            }

            var currentLive = null;
            var currentOffset = this.serviceTimeOffset + (diff / 1000);
            $.each(this.items, function (i, item) {
                item.e.removeClass('live');
                if ((currentOffset < 0 && item.offset >= currentOffset) || (currentOffset > 0 && item.offset <= currentOffset)) {
                    _this.elements.live_tag.css('height', (item.e.height() - 1) + 'px').css('top', (item.e.position().top + 1) + 'px').show();
                    currentLive = item;
                }
            });

            if (currentLive != null) {
                currentLive.e.addClass('live');
            }

            if (this.serviceEnd) {
                if ((this.serviceDate.valueOf() - d.valueOf()) < 0 && (d.valueOf() - this.serviceEnd.valueOf()) < 0) {
                    this.elements.clock_current.addClass('live');
                } else {
                    this.elements.clock_current.removeClass('live');
                    this.elements.live_tag.hide();
                }
            }
        }
    };
    return LiveApp;
})();

var liveApp = new LiveApp();
$(function () {
    liveApp.init();
});
//# sourceMappingURL=LiveApp.js.map
