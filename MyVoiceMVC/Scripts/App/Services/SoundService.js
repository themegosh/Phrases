(function () {
    "use strict";
    angular.module("Phrases").factory("SoundService", ['PhrasesService', '$rootScope', function (ps, $rootScope) {
        var ss = {};

        ss.forceReload = false;
        ss.isInit = false;

        ss.currentPhrase = null;
        ss.currentPhraseProgress = null;
        ss.audio = null;
        ss.audioSrc = null;

        ss.init = function () {
            ss.isInit = true;
            ss.audio = $('#audio-ele')[0];
            ss.audioSrc = $('#audio-src');
            //ss.audio.addEventListener('timeupdate', ss.onAudioTimeUpdate);
        }

        //this entire thing is gross. rewrite it at some point
        ss.playSound = function (phrase, shouldReload, $event) {

            if (!ss.isInit) { //save cycles, init once
                ss.init();
            }

            //get the fab started
            $('.fab').fadeIn();

            //if there was a previous phrase, stop the animation and reset it
            if (ss.currentPhraseProgress)
                ss.currentPhraseProgress.stop().width('0%');

            //set new bindings
            ss.currentPhrase = phrase;
            if ($event)
                ss.currentPhraseProgress = $($event.currentTarget).find('.phrase-progress').first();
            else
                ss.currentPhraseProgress = null;

            var url = "/api/audio/GetAudio/" + ss.currentPhrase.guid + "?userGuid=" + ps.user.userGuid;
            
            //load the audio if needed
            if (ss.audioSrc.attr("src") != url || shouldReload || ss.forceReload) {
                ss.currentPhrase.loading = true;
                ss.audioSrc.attr("src", url);
                ss.audio.load();//load new file
                ss.forceReload = false;
            }
            
            //if it was already playing pause it
            ss.audio.pause();
            ss.audio.currentTime = 0; //start over
            ss.audio.oncanplaythrough = ss.onCanPlayThrough;//once the audio is loaded, play
            
            ss.audio.addEventListener('ended', function () {
                ss.updateFab();
            });
        }

        ss.onCanPlayThrough = function () {
            if (ss.currentPhraseProgress) {
                ss.currentPhraseProgress.stop().width('0%');
                ss.currentPhraseProgress.animate({ width: '100%' }, ss.audio.duration * 1000, 'linear', function () { $(this).width('0%') });
            }
            ss.currentPhrase.loading = false;
            ss.audio.play().catch(function (e) {
                //iOS & android [ajax tts to audio > api success > play new audio] doesnt work (for the first ever play)... needs to hit play at least once first
                //workaround: preload 1s length, muted audio file then ajax request TODO
                showNotification("Hey!", "Looks like your browser blocked the audio playback. Click [Play] again!", "info");
                console.log("Browser blocked the initial audio playback... TODO: FIX THIS.");
                console.log(e);
            });
            ss.updateFab();
            if (!$rootScope.$$phase)
                $rootScope.$apply();
        }

        //ss.onAudioTimeUpdate = function () {
        //    if (ss.audio.currentTime != ss.audio.duration && ss.audio.currentTime != 0) {
        //        ss.currentPhrase.playProgress = ((ss.audio.currentTime / ss.audio.duration) * 100) + "%";
        //    } else {
        //        ss.currentPhrase.playProgress = null;
        //    }

        //    //console.log("TIME UPDATED: " + ss.audio.currentTime + " OF " + ss.audio.duration + ": " + ss.currentPhrase.playProgress);

        //    $rootScope.$apply();
        //}

        ss.playPause = function () {
            if (ss.audio.duration > 0 && !ss.audio.paused) {
                if (ss.currentPhraseProgress)
                    ss.currentPhraseProgress.stop();
                ss.audio.pause();
            } else {
                ss.audio.currentTime = 0;
                ss.audio.oncanplaythrough = ss.onCanPlayThrough();
            }
            ss.updateFab();
        }

        ss.updateFab = function () {
            if (ss.audio.paused) {
                $('.fab').html('<i class="fa fa-play" aria-hidden="true"></i>');
            } else {
                $('.fab').html('<i class="fa fa-pause" aria-hidden="true"></i>');
            }
        }
        
        
        return ss;
    }]);
})();