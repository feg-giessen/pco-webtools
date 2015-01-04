/// <reference path="typings/jquery/jquery.d.ts" /> 

interface ISong {
    itemid: string;
    b: JQuery;
    e: JQuery;
}

interface IEventItem {
    e: JQuery;
    offset: number;
}

class LiveApp {
    private songs: Array<ISong> = [];
    private items: Array<IEventItem> = [];
    private serviceDate: Date;
    private serviceEnd: Date;
    private serviceTimeOffset: number;

    private elements = {
        clock_current: null,
        clock_pre: null,
        clock_past: null,
        live_tag: null,
        offset_tag: null
    };

    constructor() {
        this.serviceTimeOffset = 0;
    }

    public toggleSong(songElement, itemid) {
        if (songElement.is(':visible')) {
            songElement.slideUp();
            localStorage.setItem('song_' + itemid, "0");
        } else {
            songElement.slideDown();
            localStorage.removeItem('song_' + itemid);
        }
    }

    public hideSongs() {
        $.each(this.songs, (i, song) => {
            if (song.e.is(':visible')) {
                song.e.slideUp();
                localStorage.setItem('song_' + song.itemid, "0");
            }
        });
    }

    public showSongs() {
        $.each(this.songs, (i, song) => {
            if (!song.e.is(':visible')) {
                song.e.slideDown();
                localStorage.removeItem('song_' + song.itemid);
            }
        });
    }

    public init () {
        var songElements = $('.song-details');

        $.each(songElements, (i, songElement: JQuery) => {
            var $songElement = $(songElement);
            var itemtd = $songElement.parents('td');
            var b = itemtd.find('.song-toggle');

            var song: ISong = {
                itemid: $songElement.parents('tr').data('id'),
                b: b,
                e: $songElement
            };

            this.songs.push(song);

            b.on('click', () => {
                this.toggleSong($songElement, song.itemid);
            });

            if (localStorage.getItem('song_' + song.itemid) == 0) {
                $songElement.hide();
            }
        });

        var items = $('table.live tbody tr');
        $.each(items, (i, itemElement) => {
            var $itemElement = $(itemElement);
            var item: IEventItem = {
                e: $itemElement,
                offset: parseInt($itemElement.data('time-offset'))
            };

            this.items.push(item);
        });

        this.elements.clock_current = $('.plan-clock .current-clock');
        this.elements.clock_pre = $('.plan-clock .to-service-clock');
        this.elements.clock_past = $('.plan-clock .in-service-clock');
        this.elements.live_tag = $('.live-tag');
        this.elements.offset_tag = $('.offset-tag');
    }

    public updateTimes() {
        var d = new Date(Date.now());
        var h = d.getHours();
        var m = d.getMinutes();
        var s = d.getSeconds();

        var hours: number;
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
            $.each(this.items, (i, item) => {
                item.e.removeClass('live');
                if ((currentOffset < 0 && item.offset >= currentOffset)
                    || (currentOffset > 0 && item.offset <= currentOffset)) {
                    this.elements.live_tag
                        .css('height', (item.e.height() - 1) + 'px')
                        .css('top', (item.e.position().top + 1) + 'px')
                        .show();
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
    }
}

var liveApp = new LiveApp();
$(() => {
    liveApp.init();
});