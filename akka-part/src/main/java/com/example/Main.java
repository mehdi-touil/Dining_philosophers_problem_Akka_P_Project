package com.example;

import akka.actor.AbstractActor;
import akka.actor.ActorRef;
import akka.actor.ActorSystem;
import akka.actor.Props;
import scala.concurrent.duration.Duration;

import java.util.concurrent.TimeUnit;

class ForkActor extends AbstractActor {
    private boolean isAvailable = true;

    @Override
    public Receive createReceive() {
        return receiveBuilder()
                .matchEquals("request", msg -> {
                    if (isAvailable) {
                        isAvailable = false;
                        getSender().tell("granted", getSelf());
                    } else {
                        getSender().tell("not_granted", getSelf());
                    }
                })
                .matchEquals("release", msg -> {
                    isAvailable = true;
                    getSender().tell("released", getSelf());
                })
                .build();
    }

    public static Props props() {
        return Props.create(ForkActor.class);
    }
}

class PhilosopherActor extends AbstractActor {
    private enum State {
        THINKING, WAITING_FIRST, WAITING_SECOND, EATING
    }

    private final String name;
    private final ActorRef leftFork;
    private final ActorRef rightFork;
    private final boolean reverseOrder;
    private State state = State.THINKING;
    private ActorRef firstFork;
    private ActorRef secondFork;

    public PhilosopherActor(String name, ActorRef leftFork, ActorRef rightFork, boolean reverseOrder) {
        this.name = name;
        this.leftFork = leftFork;
        this.rightFork = rightFork;
        this.reverseOrder = reverseOrder;
        this.firstFork = reverseOrder ? rightFork : leftFork;
        this.secondFork = reverseOrder ? leftFork : rightFork;
    }

    @Override
    public Receive createReceive() {
        return receiveBuilder()
                .matchEquals("start", msg -> startThinking())
                .matchEquals("start_eating", msg -> tryToEat())
                .matchEquals("granted", this::handleGranted)
                .matchEquals("not_granted", this::handleNotGranted)
                .matchEquals("release", msg -> releaseForks())
                .build();
    }

    private void startThinking() {
        System.out.println(name + " is thinking.");
        state = State.THINKING;
        getContext().system().scheduler().scheduleOnce(
                Duration.create((long) (Math.random() * 1000), TimeUnit.MILLISECONDS),
                getSelf(),
                "start_eating",
                getContext().system().dispatcher(),
                getSelf());
    }

    private void tryToEat() {
        if (state != State.THINKING)
            return;
        state = State.WAITING_FIRST;
        firstFork.tell("request", getSelf());
    }

    private void handleGranted(String msg) {
        if (state == State.WAITING_FIRST) {
            state = State.WAITING_SECOND;
            // try {
            //     Thread.sleep(10000);
            // } catch (Exception e) {

            //     System.out.println(e);
            // }
            secondFork.tell("request", getSelf());
        } else if (state == State.WAITING_SECOND) {
            state = State.EATING;
            System.out.println(name + " is eating.");
            getContext().system().scheduler().scheduleOnce(
                    Duration.create((long) (Math.random() * 1000), TimeUnit.MILLISECONDS),
                    getSelf(),
                    "release",
                    getContext().system().dispatcher(),
                    getSelf());
        }
    }

    private void handleNotGranted(String msg) {
        if (state == State.WAITING_FIRST) {
            System.out.println(name + " couldn't get first fork. Retrying...");
            getContext().system().scheduler().scheduleOnce(
                    Duration.create(1000, TimeUnit.MILLISECONDS),
                    getSelf(),
                    "start_eating",
                    getContext().system().dispatcher(),
                    getSelf());
            state = State.THINKING;
        } else if (state == State.WAITING_SECOND) {

            System.out.println(name + " couldn't get second fork. Releasing first fork...");
            firstFork.tell("release", getSelf());
            state = State.THINKING;
            getContext().system().scheduler().scheduleOnce(
                    Duration.create(1000, TimeUnit.MILLISECONDS),
                    getSelf(),
                    "start_eating",
                    getContext().system().dispatcher(),
                    getSelf());
        }
    }

    private void releaseForks() {
        if (state == State.EATING) {
            firstFork.tell("release", getSelf());
            secondFork.tell("release", getSelf());
            System.out.println(name + " released both forks.");
            state = State.THINKING;
            startThinking();
        }
    }

    public static Props props(String name, ActorRef leftFork, ActorRef rightFork, boolean reverseOrder) {
        return Props.create(PhilosopherActor.class,
                () -> new PhilosopherActor(name, leftFork, rightFork, reverseOrder));
    }
}

public class Main {
    public static void main(String[] args) {
        ActorSystem system = ActorSystem.create("DiningPhilosophersSystem");

        ActorRef fork1 = system.actorOf(ForkActor.props(), "fork1");
        ActorRef fork2 = system.actorOf(ForkActor.props(), "fork2");
        ActorRef fork3 = system.actorOf(ForkActor.props(), "fork3");
        ActorRef fork4 = system.actorOf(ForkActor.props(), "fork4");
        ActorRef fork5 = system.actorOf(ForkActor.props(), "fork5");

        ActorRef philosopher1 = system.actorOf(PhilosopherActor.props("Philosopher1", fork1, fork2, false),
                "philosopher1");
        ActorRef philosopher2 = system.actorOf(PhilosopherActor.props("Philosopher2", fork2, fork3, false),
                "philosopher2");
        ActorRef philosopher3 = system.actorOf(PhilosopherActor.props("Philosopher3", fork3, fork4, false),
                "philosopher3");
        ActorRef philosopher4 = system.actorOf(PhilosopherActor.props("Philosopher4", fork4, fork5, false),
                "philosopher4");
        ActorRef philosopher5 = system.actorOf(PhilosopherActor.props("Philosopher5", fork5, fork1, true),
                "philosopher5");

        philosopher1.tell("start", ActorRef.noSender());
        philosopher2.tell("start", ActorRef.noSender());
        philosopher3.tell("start", ActorRef.noSender());
        philosopher4.tell("start", ActorRef.noSender());
        philosopher5.tell("start", ActorRef.noSender());

        try {
            Thread.sleep(10000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        system.terminate();
    }
}